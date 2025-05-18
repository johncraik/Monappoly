using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Boards.Spaces;
using MonappolyLibrary.GameServices.Boards;

namespace Monappoly_ASP.Pages.Objects.Boards.Spaces.Property;

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

    public class PropertySpaceInputModel
    {
        public string Name { get; set; }
        public uint Cost { get; set; }
        public PropertyType PropertyType { get; set; }
        public PropertySet? PropertySet { get; set; }

        public PropertySpaceInputModel()
        {
        }

        public PropertySpaceInputModel(PropertyBoardSpace space)
        {
            Name = space.Name;
            Cost = space.Cost;
            PropertyType = space.PropertyType;
            PropertySet = space.PropertySet;
        }
        
        public void FillSpace(PropertyBoardSpace space)
        {
            space.Name = Name;
            space.Cost = Cost;
            space.PropertyType = PropertyType;
            space.PropertySet = PropertySet ?? MonappolyLibrary.GameModels.Boards.Spaces.PropertySet.None;
        }
    }
    
    [BindProperty]
    public PropertySpaceInputModel Input { get; set; }
    public List<SelectListItem> Types { get; set; }
    public List<SelectListItem> Sets { get; set; }
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
        
        Types = PropertyType.SetProperty.GetSelectList();
        Sets = PropertySet.Brown.GetSelectList(true, "None (Not a Set Property)");
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
            Input = new PropertySpaceInputModel();
            return Page();
        }

        var space = await _boardSpaceService.FindPropertySpace(id);
        if (space == null) return new NotFoundResult();
        
        Input = new PropertySpaceInputModel(space);
        return Page();
    }

    public async Task<IActionResult> OnPost(int boardId, int id, int index, bool replace = false)
    {
        Replacing = replace;
        SetupAdding(id);
        var result = await SetupPage(boardId, index);
        if (result != null) return result;

        bool res;
        if (Adding)
        {
            var space = new PropertyBoardSpace
            {
                BoardId = boardId,
                TenantId = _userInfo.TenantId,
                BoardIndex = (uint)index
            };
            Input.FillSpace(space);
            res = await _boardSpaceService.TryAddPropertySpace(space, ModelState);
        }
        else if (Replacing)
        {
            var space = new PropertyBoardSpace
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
            var space = await _boardSpaceService.FindPropertySpace(id);
            if (space == null) return new NotFoundResult();
            
            Input.FillSpace(space);
            res = await _boardSpaceService.TryUpdatePropertySpace(space, ModelState);
        }
        
        return res ? RedirectToPage($"/Objects/Boards/{nameof(Index)}", new { boardId }) : Page();
    }
}