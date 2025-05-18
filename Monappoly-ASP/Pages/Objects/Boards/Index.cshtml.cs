using Microsoft.AspNetCore.Mvc.RazorPages;
using MonappolyLibrary.GameModels.Boards;
using MonappolyLibrary.GameModels.Boards.ViewModels;
using MonappolyLibrary.GameServices.Boards;

namespace Monappoly_ASP.Pages.Objects.Boards;

public class Index : PageModel
{
    private readonly BoardService _boardService;

    public Index(BoardService boardService)
    {
        _boardService = boardService;
    }
    
    public List<Board> Boards { get; set; }
    public BoardViewModel? CurrentBoard { get; set; }

    public async Task SetupPage(int id)
    {
        Boards = await _boardService.GetBoards();
        if (Boards.Count > 0)
        {
            var bid = Boards.FirstOrDefault()?.Id ?? 0;
            if(id > 0) bid = id;
            
            CurrentBoard = await _boardService.BuildViewModel(bid);
        }
    }
    
    public async Task OnGet(int? boardId = null)
    {
        await SetupPage(boardId ?? 0);
    }
}