using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly CardActionService _cardActionService;

    public IndexModel(ILogger<IndexModel> logger, CardActionService cardActionService)
    {
        _logger = logger;
        _cardActionService = cardActionService;
    }

    public async Task<IActionResult> OnGet()
    {
        var test = await _cardActionService.GetAllCardActions(1);
        return Page();
    }
}