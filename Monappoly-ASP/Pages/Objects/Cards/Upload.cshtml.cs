using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary.GameModels.Cards.ViewModels;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards;

public class Upload : PageModel
{
    private readonly CardService _cardService;

    public Upload(CardService cardService)
    {
        _cardService = cardService;
    }
    
    public int Id { get; set; }
    
    [BindProperty]
    public CardUploadInputModel Input { get; set; }
    public List<SelectListItem> CardTypes { get; set; }
    public List<SelectListItem> CardDecks { get; set; }

    public async Task<IActionResult?> SetupPage(int id)
    {
        var deck = await _cardService.FindDeck(id);
        if(deck == null || !deck.IsModifiable())
        {
            return RedirectToPage($"/{nameof(MonopolyDefaults)}");
        }

        Id = id;
        CardTypes = (await _cardService.GetTypes())
            .Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.Id.ToString()
            }).ToList();
        CardDecks = (await _cardService.GetDecks()).Where(d => d.TenantId > 0)
            .Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = d.Id == id
            }).OrderByDescending(d => d.Selected).ThenBy(d => d.Text)
            .ToList();
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
        if (!ModelState.IsValid) return Page();

        var res = await _cardService.TryUploadCards(Input, ModelState);
        return res ? RedirectToPage($"/Objects/Cards/{nameof(Index)}", new { deck = id }) : Page();
    }
}