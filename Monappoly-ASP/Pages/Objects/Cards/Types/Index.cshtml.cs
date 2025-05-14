using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Types;

public class Index : PageModel
{
    private readonly CardService _cardService;

    public Index(CardService cardService)
    {
        _cardService = cardService;
    }
    
    public List<CardType> Types { get; set; }

    public async Task SetupPage()
    {
        Types = await _cardService.GetTypes();
    }
    
    public async Task<IActionResult> OnGet()
    {
        await SetupPage();
        return Page();
    }
}