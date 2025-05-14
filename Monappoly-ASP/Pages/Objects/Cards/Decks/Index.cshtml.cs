using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Decks;

public class Index : PageModel
{
        private readonly UserInfo _userInfo;
    private readonly CardService _cardService;

    public Index(UserInfo userInfo,
        CardService cardService)
    {
        _userInfo = userInfo;
        _cardService = cardService;
    }

    public class DeckInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        
        [Required]
        public CardDeckDifficulty Difficulty { get; set; }

        public DeckInputModel()
        {
        }

        public DeckInputModel(CardDeck deck)
        {
            Id = deck.Id;
            Name = deck.Name;
            Description = deck.Description;
            Difficulty = deck.Difficulty;
        }
        
        public void Fill(CardDeck deck)
        {
            deck.Name = Name;
            deck.Description = Description ?? "";
            deck.Difficulty = Difficulty;
        }
    }
    
    [BindProperty]
    public DeckInputModel Input { get; set; }
    public List<SelectListItem> Difficulties { get; set; }
    public bool Adding { get; set; } = true;

    public void SetupAdding(int id)
    {
        Adding = id == 0;
    }

    public void SetupPage(int id)
    {
        Difficulties = CardDeckDifficulty.Default.GetSelectList();
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        SetupAdding(id);
        SetupPage(id);

        if (Adding)
        {
            Input = new DeckInputModel();
        }
        else
        {
            var deck = await _cardService.FindDeck(id);
            if(deck == null || !deck.IsModifiable())
            {
                return RedirectToPage($"/{nameof(MonopolyDefaults)}");
            }
            Input = new DeckInputModel(deck);
        }

        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        SetupAdding(id);
        SetupPage(id);
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        bool res;
        if (Adding)
        {
            var deck = new CardDeck
            {
                Name = "",
                TenantId = _userInfo.TenantId
            };
            Input.Fill(deck);
            res = await _cardService.TryAddDeck(deck, ModelState);
        }
        else
        {
            //Update card
            var deck = await _cardService.FindDeck(id);
            if(deck == null || !deck.IsModifiable())
            {
                return RedirectToPage($"/{nameof(MonopolyDefaults)}");
            }
            Input.Fill(deck);
            res = await _cardService.TryUpdateDeck(deck, ModelState);
        }

        return res ? RedirectToPage($"/Objects/Cards/{nameof(Index)}", new { deck = Input.Id }) : Page();
    }
}