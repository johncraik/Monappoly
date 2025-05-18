using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Boards.Spaces;
using MonappolyLibrary.GameServices.Boards;

namespace Monappoly_ASP.Pages.Objects.Boards.Spaces.Tax;

public class Edit : PageModel
{
    private readonly BoardSpaceService _boardSpaceService;
    private readonly BoardService _boardService;
    private readonly UserInfo _userInfo;

    public Edit(BoardSpaceService boardSpaceService,
        BoardService boardService,
        UserInfo userInfo)
    {
        _boardSpaceService = boardSpaceService;
        _boardService = boardService;
        _userInfo = userInfo;
    }

    public class TaxSpaceInputModel
    {
        public string Name { get; set; }
        public uint TaxAmount { get; set; }

        public TaxSpaceInputModel()
        {
        }

        public TaxSpaceInputModel(TaxBoardSpace space)
        {
            Name = space.Name;
            TaxAmount = space.TaxAmount;
        }
        
        public void FillSpace(TaxBoardSpace space)
        {
            space.Name = Name;
            space.TaxAmount = TaxAmount;
        }
    }
    
    [BindProperty]
    public TaxSpaceInputModel Input { get; set; }
    public bool Adding { get; set; }
    public bool Replacing { get; set; }

    public void SetupAdding(int id)
    {
        if (id == 0) Adding = true;
    }

    public async Task<IActionResult?> SetupPage(int boardId, int index)
    {
        if (Adding)
        {
            var empty = await _boardSpaceService.EmptySpace(boardId, index);
            if (!empty) return new BadRequestResult();
        }
        
        var board = await _boardService.FindBoard(boardId);
        if (!Adding && (board == null || !board.IsModifiable())) return RedirectToPage($"/{nameof(MonopolyDefaults)}");

        return null;
    }

    public async Task<IActionResult> OnGet(int boardId, int id, int index, bool replace = false)
    {
        Replacing = replace;
        SetupAdding(id);
        var result = await SetupPage(boardId, index);
        if (result != null) return result;

        if (Adding || Replacing)
        {
            Input = new TaxSpaceInputModel();
            return Page();
        }

        var space = await _boardSpaceService.FindTaxSpace(id);
        if (space == null) return new NotFoundResult();
        
        Input = new TaxSpaceInputModel(space);
        return Page();
    }

    public async Task<IActionResult> OnPost(int boardId, int id, int index, bool replace = false)
    {
        Replacing = replace;
        SetupAdding(id);
        var result = await SetupPage(boardId, index);
        if (result != null) return result;

        if (!ModelState.IsValid) return Page();

        bool res;
        if (Adding)
        {
            var space = new TaxBoardSpace
            {
                BoardId = boardId,
                TenantId = _userInfo.TenantId,
                BoardIndex = (uint)index
            };
            Input.FillSpace(space);
            res = await _boardSpaceService.TryAddTaxSpace(space, ModelState);
        }
        else if (Replacing)
        {
            var space = new TaxBoardSpace
            {
                BoardId = boardId,
                TenantId = _userInfo.TenantId,
                BoardIndex = (uint)index
            };
            Input.FillSpace(space);
            var currentSpace = await _boardSpaceService.FindSpace(boardId, index);
            if (currentSpace == null) return new NotFoundResult();
            
            res = await _boardSpaceService.TryReplaceSpace(currentSpace, space, ModelState);
        }
        else
        {
            var space = await _boardSpaceService.FindTaxSpace(id);
            if (space == null) return new NotFoundResult();
            
            Input.FillSpace(space);
            res = await _boardSpaceService.TryUpdateTaxSpace(space, ModelState);
        }
        
        return res ? RedirectToPage($"/Objects/Boards/{nameof(Index)}", new { boardId }) : Page();
    }
}