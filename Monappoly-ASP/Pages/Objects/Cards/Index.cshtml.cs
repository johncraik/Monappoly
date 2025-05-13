using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MonappolyLibrary;
using MonappolyLibrary.Data.Defaults.Dictionaries;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.ViewModels;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards;

public class Index : PageModel
{
    private readonly CardService _cardService;
    private readonly UserInfo _userInfo;

    public Index(CardService cardService,
        UserInfo userInfo)
    {
        _cardService = cardService;
        _userInfo = userInfo;
    }
    
    public CardDeck CurrentDeck { get; set; }
    public List<CardDeck> Decks { get; set; }
    public List<CardViewModel> Cards { get; set; }

    public async Task<IActionResult?> Setup(int currentDeckId)
    {
        Decks = await _cardService.GetDecks();
        if (Decks.Count > 0)
        {
            if (currentDeckId == 0)
            {
                CurrentDeck = Decks[0];
            }
            else
            {
                var deck = Decks.FirstOrDefault(d => d.Id == currentDeckId);
                if (deck == null) return new NotFoundResult();
                
                CurrentDeck = deck;
            }
            
            Cards = await _cardService.GetCards(CurrentDeck.Id);
        }
        else
        {
            Cards = [];
        }

        return null;
    }
    
    public async Task<IActionResult> OnGet(int? deck)
    {
        var rtn = await Setup(deck ?? 0);
        return rtn ?? Page();
    }
}