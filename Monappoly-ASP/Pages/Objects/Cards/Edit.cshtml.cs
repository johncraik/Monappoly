using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards;

public class Edit : PageModel
{
    private readonly UserInfo _userInfo;
    private readonly CardService _cardService;

    public Edit(UserInfo userInfo,
        CardService cardService)
    {
        _userInfo = userInfo;
        _cardService = cardService;
    }

    public class CardInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [DisplayName("Card Type")]
        public int CardTypeId { get; set; }
        [Required]
        [DisplayName("Card Deck")]
        public int CardDeckId { get; set; }

        public CardInputModel()
        {
        }

        public CardInputModel(Card card)
        {
            Id = card.Id;
            Text = card.Text;
            CardTypeId = card.CardTypeId;
            CardDeckId = card.CardDeckId;
        }
        
        public void Fill(Card card)
        {
            card.Text = Text;
            card.CardTypeId = CardTypeId;
            card.CardDeckId = CardDeckId;
        }
    }
    
    [BindProperty]
    public CardInputModel Input { get; set; }
    public List<SelectListItem> CardTypes { get; set; }
    public List<SelectListItem> CardDecks { get; set; }
    public bool Adding { get; set; } = true;

    public void SetupAdding(int id)
    {
        Adding = id == 0;
    }

    public async Task<IActionResult?> SetupPage(int id, int deckId)
    {
        var deck = await _cardService.FindDeck(deckId);
        if(deck == null || !deck.IsModifiable())
        {
            return RedirectToPage($"/{nameof(MonopolyDefaults)}");
        }
        
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
                Selected = d.Id == deckId
            }).OrderByDescending(d => d.Selected).ThenBy(d => d.Text)
            .ToList();
        return null;
    }
    
    public async Task<IActionResult> OnGet(int deckId, int id)
    {
        SetupAdding(id);
        var rtn = await SetupPage(id, deckId);
        if (rtn != null) return rtn;

        if (Adding)
        {
            Input = new CardInputModel();
        }
        else
        {
            var card = await _cardService.FindCard(id);
            if(card == null || !card.IsModifiable())
            {
                return RedirectToPage($"/{nameof(MonopolyDefaults)}");
            }
            Input = new CardInputModel(card);
        }

        return Page();
    }

    public async Task<IActionResult> OnPost(int deckId, int id)
    {
        SetupAdding(id);
        var rtn = await SetupPage(id, deckId);
        if (rtn != null) return rtn;
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        bool res;
        if (Adding)
        {
            var card = new Card
            {
                Text = "",
                TenantId = _userInfo.TenantId
            };
            Input.Fill(card);
            res = await _cardService.TryAddCard(card, ModelState);
        }
        else
        {
            //Update card
            var card = await _cardService.FindCard(id);
            if(card == null || !card.IsModifiable())
            {
                return RedirectToPage($"/{nameof(MonopolyDefaults)}");
            }
            Input.Fill(card);
            res = await _cardService.TryUpdateCard(card, ModelState);
        }

        return res ? RedirectToPage($"/Objects/Cards/{nameof(Index)}", new { deck = Input.CardDeckId }) : Page();
    }
}