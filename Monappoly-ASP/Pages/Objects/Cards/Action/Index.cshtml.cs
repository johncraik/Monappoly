using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Action;

public class Index : PageModel
{
    private readonly CardActionService _cardActionService;
    private readonly CardService _cardService;
    
    public Index(CardActionService cardActionService, 
        CardService cardService)
    {
        _cardActionService = cardActionService;
        _cardService = cardService;
    }
    
    public List<(ActionGroupViewModel Group, List<ActionViewModel> Actions)> CardActions { get; set; }
    public Card Card { get; set; }

    public async Task<IActionResult?> SetupPage(int cardId)
    {
        var card = await _cardService.FindCard(cardId);
        if(card == null) return new NotFoundResult();

        Card = card;
        CardActions = await _cardActionService.GetGroupViewModels(cardId);
        return null;
    }
    
    public async Task<IActionResult> OnGet(int cardId)
    {
        await SetupPage(cardId);
        return Page();
    }
}