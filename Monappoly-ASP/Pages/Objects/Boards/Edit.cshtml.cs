using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.GameModels.Boards;
using MonappolyLibrary.GameServices.Boards;

namespace Monappoly_ASP.Pages.Objects.Boards;

public class Edit : PageModel
{
    private readonly BoardService _boardService;
    private readonly BuildingGroupService _buildingGroupService;
    private readonly UserInfo _userInfo;

    public Edit(BoardService boardService,
        BuildingGroupService buildingGroupService,
        UserInfo userInfo)
    {
        _boardService = boardService;
        _buildingGroupService = buildingGroupService;
        _userInfo = userInfo;
    }

    public class BoardInputModel
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [DisplayName("Building Group")]
        public int BuildingGroupId { get; set; }

        public BoardInputModel()
        {
        }

        public BoardInputModel(Board board)
        {
            Name = board.Name;
            Description = board.Description;
            BuildingGroupId = board.BuildingGroupId;    
        }

        public void Fill(Board board)
        {
            board.Name = Name;
            board.Description = Description;
            board.BuildingGroupId = BuildingGroupId;
        }
    }
    
    [BindProperty]
    public BoardInputModel Input { get; set; }
    public List<SelectListItem> BuildingGroups { get; set; }
    public bool Adding { get; set; }

    public void SetupAdding(int id)
    {
        if (id == 0) Adding = true;
    }

    public async Task SetupPage()
    {
        BuildingGroups = await _buildingGroupService.GetBuildingGroupsDropdown();
    }

    public async Task<IActionResult> OnGet(int id)
    {
        SetupAdding(id);
        await SetupPage();

        if (Adding)
        {
            Input = new BoardInputModel();
            return Page();
        }
        
        var board = await _boardService.FindBoard(id);
        if (board == null || !board.IsModifiable()) return RedirectToPage($"/{nameof(MonopolyDefaults)}");
        
        Input = new BoardInputModel(board);
        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        SetupAdding(id);
        await SetupPage();
        if (!ModelState.IsValid) return Page();

        bool res;
        if (Adding)
        {
            var board = new Board
            {
                Name = "",
                TenantId = _userInfo.TenantId
            };
            Input.Fill(board);
            res = await _boardService.TryAddBoard(board, ModelState);
        }
        else
        {
            var board = await _boardService.FindBoard(id);
            if(board == null || !board.IsModifiable()) return RedirectToPage($"/{nameof(MonopolyDefaults)}");
            Input.Fill(board);
            res = await _boardService.TryUpdateBoard(board, ModelState);
        }

        return res ? RedirectToPage($"/Objects/Boards/{nameof(Index)}") : Page();
    }
}