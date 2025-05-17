using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Action;

public class Group : PageModel
{
    private readonly CardActionService _cardActionService;
    private readonly CardService _cardService;

    public Group(CardActionService cardActionService,
        CardService cardService)
    {
        _cardActionService = cardActionService;
        _cardService = cardService;
    }
    
    public Card Card { get; set; }
    public List<SelectListItem> Players { get; set; }
    public List<SelectListItem> LengthTypes { get; set; }
    public List<SelectListItem> PlayConditions { get; set; }
    
    [BindProperty]
    public ActionGroupViewModel Input { get; set; }
    public bool Adding { get; set; }

    public void SetupAdding(int id)
    {
        if (id == 0) Adding = true;
    }
    
    public async Task<IActionResult?> SetupPage(int cardId, int groupId)
    {
        var card = await _cardService.FindCard(cardId);
        if(card == null) return new NotFoundResult();

        Players = ObjectPlayer.All.GetSelectList();
        LengthTypes = ActionLengthType.Default.GetSelectList(true, "Default");
        PlayConditions = ActionPlayCondition.Default.GetSelectList(true, "Default");
        
        Card = card;
        return null;
    }

    public async Task<IActionResult> OnGet(int cardId, int groupId)
    {
        SetupAdding(groupId); 
        var rtn = await SetupPage(cardId, groupId);
        if (rtn != null) return rtn;

        if (Adding)
        {
            Input = new ActionGroupViewModel(cardId);
            return Page();
        }
        
        var group = await _cardActionService.FindCardActionGroup(cardId);
        if(group == null) return new NotFoundResult();
        
        Input = new ActionGroupViewModel(group);
        return Page();
    }
    
    public async Task<IActionResult> OnPost(int cardId, int groupId)
    {
        SetupAdding(groupId);
        await SetupPage(cardId, Input.Id);
        if (!ModelState.IsValid) return Page();

        bool res;
        if (Adding)
        {
            Input.CardId = cardId;
            res = await _cardActionService.AddCardActionGroup(Input, ModelState);
        }
        else
        {
            Input.Id = groupId;
            Input.CardId = cardId;
            res = await _cardActionService.UpdateCardActionGroup(Input, ModelState);
        }
        
        return res ? RedirectToPage($"/Objects/Cards/Action/Index", new { cardId }) : Page();
    }
}