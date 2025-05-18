using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Boards.Spaces;
using MonappolyLibrary.GameServices.Boards;

namespace Monappoly_ASP.Pages.Objects.Boards.Spaces.Generic;

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

    public class GenericSpaceInputModel
    {
        public string Name { get; set; }
        public GenericSpaceAction Action { get; set; }

        public GenericSpaceInputModel()
        {
        }

        public GenericSpaceInputModel(GenericBoardSpace space)
        {
            Name = space.Name;
            Action = space.Action;
        }
        
        public void FillSpace(GenericBoardSpace space)
        {
            space.Name = Name;
            space.Action = Action;
        }
    }
    
    [BindProperty]
    public GenericSpaceInputModel Input { get; set; }
    public List<SelectListItem> Actions { get; set; }
    public bool Adding { get; set; }
    public bool Replacing { get; set; }

    public void SetupAdding(int id)
    {
        if (id == 0 && !Replacing) Adding = true;
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
        
        Actions = GenericSpaceAction.Go.GetSelectList();
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
            Input = new GenericSpaceInputModel();
            return Page();
        }

        var space = await _boardSpaceService.FindGenericSpace(id);
        if (space == null) return new NotFoundResult();
        
        Input = new GenericSpaceInputModel(space);
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
            var space = new GenericBoardSpace
            {
                BoardId = boardId,
                TenantId = _userInfo.TenantId,
                BoardIndex = (uint)index
            };
            Input.FillSpace(space);
            res = await _boardSpaceService.TryAddGenericSpace(space, ModelState);
        }
        else if (Replacing)
        {
            var space = new GenericBoardSpace
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
            var space = await _boardSpaceService.FindGenericSpace(id);
            if (space == null) return new NotFoundResult();
            
            Input.FillSpace(space);
            res = await _boardSpaceService.TryUpdateGenericSpace(space, ModelState);
        }
        
        return res ? RedirectToPage($"/Objects/Boards/{nameof(Index)}", new { boardId }) : Page();
    }
}