using Microsoft.AspNetCore.Mvc;
using MonappolyLibrary;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Controllers;

public class CardController : Controller
{
    private readonly UserInfo _userInfo;
    private readonly CardService _cardService;

    public CardController(UserInfo userInfo,
        CardService cardService)
    {
        _userInfo = userInfo;
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCardsTable(int deck)
    {
        var model = await _cardService.GetCards(deck);
        return PartialView("Objects/Cards/_CardsTable", model);
    }

    [HttpPost]
    public async Task<bool> DeleteCards(int deckId)
    {
        //Check deck exists:
        var deck = await _cardService.FindDeck(deckId);
        if(deck == null) return false;

        //Check deck can be deleted:
        if (!deck.IsDeletable()) return false;

        await _cardService.DeleteAllCards(deckId);
        return true;
    }

    [HttpPost]
    public async Task<bool> DeleteCard(int id) => await _cardService.TryDeleteCard(id);

    [HttpPost]
    public async Task<bool> DeleteDeck(int id) => await _cardService.TryDeleteDeck(id);
    
    [HttpPost]
    public async Task<bool> DeleteType(int id) => await _cardService.TryDeleteType(id);
}