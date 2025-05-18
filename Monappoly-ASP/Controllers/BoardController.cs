using Microsoft.AspNetCore.Mvc;
using MonappolyLibrary.Data;
using MonappolyLibrary.GameServices.Boards;

namespace Monappoly_ASP.Controllers;

public class BoardController : Controller
{
    private readonly BoardService _boardService;

    public BoardController(BoardService boardService)
    {
        _boardService = boardService;
    }

    public async Task<IActionResult> GetBoardViewPartial(int boardId)
    {
        var model = await _boardService.BuildViewModel(boardId);
        return PartialView("Objects/Boards/_BoardViewPartial", model);
    }

    public async Task<bool> DeleteBoard(int id)
    {
        var board = await _boardService.FindBoard(id);
        if (board == null) return false;

        return await _boardService.TryDeleteBoard(board);
    }
}