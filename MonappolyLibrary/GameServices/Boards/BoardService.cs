using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;
using MonappolyLibrary.GameModels.Boards;
using MonappolyLibrary.GameModels.Boards.Spaces;
using MonappolyLibrary.GameModels.Boards.ViewModels;

namespace MonappolyLibrary.GameServices.Boards;

public class BoardService
{
    private readonly MonappolyDbContext _context;
    private readonly UserInfo _userInfo;
    private readonly BoardSpaceService _boardSpaceService;
    private readonly BuildingGroupService _buildingGroupService;

    public BoardService(MonappolyDbContext context,
        UserInfo userInfo,
        BoardSpaceService boardSpaceService,
        BuildingGroupService buildingGroupService)
    {
        _context = context;
        _userInfo = userInfo;
        _boardSpaceService = boardSpaceService;
        _buildingGroupService = buildingGroupService;
    }

    public async Task<List<Board>> GetBoards() => 
        await _context.Boards.Include(b => b.BuildingGroup)
            .OrderByDescending(b => b.TenantId).ThenBy(b => b.Name)
            .ToListAsync();
    
    public async Task<Board?> FindBoard(int boardId) => 
        await _context.Boards.Include(b => b.BuildingGroup)
            .FirstOrDefaultAsync(b => b.Id == boardId);

    private async Task ValidateBoard(Board board, ModelStateDictionary modelState)
    {
        var exists = await _context.Boards.AnyAsync(b => b.Name == board.Name && b.Id != board.Id);
        if (exists)
        {
            modelState.AddModelError($"Input.{nameof(board.Name)}", "Board with this name already exists.");
        }
        
        var validGroup = await _context.BuildingGroups.AnyAsync(bg => bg.Id == board.BuildingGroupId);
        if (!validGroup)
        {
            modelState.AddModelError($"Input.{nameof(board.BuildingGroupId)}", "Building group does not exist.");
        }
    }
    
    public async Task<bool> TryAddBoard(Board board, ModelStateDictionary modelState)
    {
        await ValidateBoard(board, modelState);
        if (!modelState.IsValid) return false;

        board.FillCreated(_userInfo);
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdateBoard(Board board, ModelStateDictionary modelState)
    {
        await ValidateBoard(board, modelState);
        if (!modelState.IsValid) return false;

        board.FillModified(_userInfo);
        _context.Boards.Update(board);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TryDeleteBoard(Board board)
    {
        if(!board.IsDeletable()) return false;

        var spaces = await GetBoardSpaces(board.Id);
        foreach (var space in spaces)
        {
            _boardSpaceService.DeleteSpace(space);
        }

        board.FillDeleted(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<List<IBoardSpace>> GetBoardSpaces(int boardId)
    {
        var generic = await _context.GenericBoardSpaces.Where(g => g.BoardId == boardId)
            .ToListAsync();
        var taxSpaces = await _context.TaxBoardSpaces.Where(t => t.BoardId == boardId)
            .ToListAsync();
        var cardSpaces = await _context.CardBoardSpaces.Include(c => c.CardType)
            .Where(c => c.BoardId == boardId)
            .ToListAsync();
        var propertySpaces = await _context.PropertyBoardSpaces.Where(p => p.BoardId == boardId)
            .ToListAsync();
        
        var spaces = new List<IBoardSpace>();
        spaces.AddRange(generic);
        spaces.AddRange(taxSpaces);
        spaces.AddRange(cardSpaces);
        spaces.AddRange(propertySpaces);
        
        return spaces.OrderBy(s => s.BoardIndex).ToList();
    }


    public async Task<BoardViewModel?> BuildViewModel(int boardId)
    {
        var board = await FindBoard(boardId);
        if(board == null) return null;

        var buildings = await _buildingGroupService.GetBuildings(board.BuildingGroupId);
        var spaces = await GetBoardSpaces(boardId);
        await _boardSpaceService.ValidateBoard(board, spaces);
        
        return new BoardViewModel
        {
            Board = board,
            Buildings = buildings,
            Spaces = spaces
        };
    }
}