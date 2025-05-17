using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Models;

public class ActionPageModel : PageModel
{
    private readonly CardService _cardService;

    public ActionPageModel(CardService cardService)
    {
        _cardService = cardService;
    }
    
    public Card Card { get; set; }
    public int GroupId { get; set; }
    public List<SelectListItem> Types { get; set; }

    public async Task<IActionResult?> SetupCard(int cardId, int groupId)
    {
        var card = await _cardService.FindCard(cardId);
        if(card == null) return new NotFoundResult();
        
        Card = card;
        GroupId = groupId;
        return null;
    }
}