using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Types;

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

    public class CardTypeInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Colour { get; set; }
        [Required]
        [DisplayName("Play Condition")]
        public CardTypeCondition PlayCondition { get; set; }
        [Required]
        [DisplayName("Rule")]
        public CardTypeRule TypeRule { get; set; }

        public CardTypeInputModel()
        {
        }

        public CardTypeInputModel(CardType type)
        {
            Id = type.Id;
            Name = type.Name;
            Description = type.Description;
            Colour = type.Colour;
            PlayCondition = type.PlayCondition;
            TypeRule = type.TypeRule;
        }
        
        public void Fill(CardType type)
        {
            type.Name = Name;
            type.Description = Description;
            type.Colour = Colour;
            type.PlayCondition = PlayCondition;
            type.TypeRule = TypeRule;
        }
    }
    
    [BindProperty]
    public CardTypeInputModel Input { get; set; }
    public List<SelectListItem> PlayConditions { get; set; }
    public List<SelectListItem> TypeRules { get; set; }
    public bool Adding { get; set; } = true;

    public void SetupAdding(int id)
    {
        Adding = id == 0;
    }

    public void SetupPage(int id)
    {
        PlayConditions = CardTypeCondition.Default.GetSelectList();
        TypeRules = CardTypeRule.Default.GetSelectList();
    }
    
    public async Task<IActionResult> OnGet(int id)
    {
        SetupAdding(id);
        SetupPage(id);

        if (Adding)
        {
            Input = new CardTypeInputModel();
        }
        else
        {
            var type = await _cardService.FindType(id);
            if(type == null || !type.IsModifiable())
            {
                return RedirectToPage($"/{nameof(MonopolyDefaults)}");
            }
            Input = new CardTypeInputModel(type);
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
            var type = new CardType()
            {
                Name = "",
                TenantId = _userInfo.TenantId
            };
            Input.Fill(type);
            res = await _cardService.TryAddType(type, ModelState);
        }
        else
        {
            //Update card
            var type = await _cardService.FindType(id);
            if(type == null || !type.IsModifiable())
            {
                return RedirectToPage($"/{nameof(MonopolyDefaults)}");
            }
            Input.Fill(type);
            res = await _cardService.TryUpdateType(type, ModelState);
        }

        return res ? RedirectToPage($"/Objects/Cards/Types/{nameof(Index)}") : Page();
    }
}