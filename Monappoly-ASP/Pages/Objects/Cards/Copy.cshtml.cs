using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.ViewModels;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards;

public class Copy : PageModel
{
    private readonly CardService _cardService;
    private readonly CardActionService _cardActionService;

    public Copy(CardService cardService, 
        CardActionService cardActionService)
    {
        _cardService = cardService;
        _cardActionService = cardActionService;
    }

    public bool IsDefaultDeck { get; set; }
    public CardDeck Deck { get; set; }
    public List<CardViewModel> Cards { get; set; }
    
    public List<SelectListItem> CardDecks { get; set; }
    [BindProperty]
    [DisplayName("Target Deck")]
    public int TargetDeckId { get; set; }
    [BindProperty]
    public bool IsCopy { get; set; } = false;
    
    public async Task<IActionResult?> SetupPage(int id)
    {
        var deck = await _cardService.FindDeck(id);
        if (deck == null) return new NotFoundResult();
        Deck = deck;

        if (!Deck.IsModifiable()) IsDefaultDeck = true;
        
        Cards = await _cardService.GetCards(id);
        CardDecks = (await _cardService.GetDecks())
            .Where(d => d.TenantId > 0 && d.Id != id)
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
        return null;
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        var rtn = await SetupPage(id);
        return rtn ?? Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        var rtn = await SetupPage(id);
        if (rtn != null) return rtn;
        
        if(!ModelState.IsValid)
        {
            return Page();
        }

        bool res;
        if (IsCopy)
        {
            res = await _cardService.CopyCards(Deck, TargetDeckId);
        }
        else
        {
            res = await _cardService.MoveCards(Deck, TargetDeckId);
        }
        
        return res ? RedirectToPage("/Objects/Cards/Index", new { deck = TargetDeckId }) : Page();
    }
}