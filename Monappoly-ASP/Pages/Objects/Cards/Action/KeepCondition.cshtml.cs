using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MonappolyLibrary;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Enums;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards;

public class KeepCondition : PageModel
{
    private readonly CardService _cardService;
    private readonly CardActionService _cardActionService;
    private readonly UserInfo _userInfo;

    public KeepCondition(CardService cardService,
        CardActionService cardActionService, 
        UserInfo userInfo)
    {
        _cardService = cardService;
        _cardActionService = cardActionService;
        _userInfo = userInfo;
    }

    public class KeepActionConditionInputModel
    {
        [DisplayName("Keep Until Needed?")]
        public bool IsUntilNeeded { get; set; }
        [DisplayName("Play Condition")]
        public CardPlayCondition PlayCondition { get; set; } = CardPlayCondition.None;
        [DisplayName("Player Condition")]
        public ObjectPlayer PlayerCondition { get; set; }
        
        [DisplayName("In-play Length Type")]
        public ActionGroupLengthType GroupLengthType { get; set; } = ActionGroupLengthType.None;
        [DisplayName("In-play Length Value")]
        public uint? LengthValue { get; set; }

        public KeepActionConditionInputModel()
        {
        }

        public KeepActionConditionInputModel(KeepActionCondition condition)
        {
            IsUntilNeeded = condition.IsUntilNeeded;
            PlayCondition = condition.PlayCondition;
            PlayerCondition = condition.PlayerCondition;
            GroupLengthType = condition.GroupLengthType;
            LengthValue = condition.LengthValue;
        }
        
        public void Fill(KeepActionCondition condition)
        {
            condition.IsUntilNeeded = IsUntilNeeded;
            condition.PlayCondition = PlayCondition;
            condition.PlayerCondition = PlayerCondition;
            condition.GroupLengthType = GroupLengthType;
            condition.LengthValue = LengthValue;
        }
    }
    
    [BindProperty]
    public KeepActionConditionInputModel Input { get; set; }
    public Card Card { get; set; }
    public List<SelectListItem> PlayConditions { get; set; }
    public List<SelectListItem> PlayerConditions { get; set; }
    public List<SelectListItem> LengthTypes { get; set; }
    public bool Adding { get; set; }
    
    public void SetupAdding(int conditionId)
    {
        Adding = conditionId == 0;
    }

    public async Task<IActionResult?> SetupPage(int cardId)
    {
        var card = await _cardService.FindCard(cardId);
        if (card == null) return new NotFoundResult();
        if (!card.IsModifiable()) return RedirectToPage($"/{nameof(MonopolyDefaults)}");
        
        Card = card;
        PlayConditions = CardPlayCondition.None.GetSelectList(true, "None");
        PlayerConditions = ObjectPlayer.None.GetSelectList();
        LengthTypes = ActionGroupLengthType.None.GetSelectList(true, "None");
        return null;
    }
    
    public async Task<IActionResult> OnGet(int cardId, int groupId, int conditionId)
    {
        SetupAdding(conditionId);
        var rtn = await SetupPage(cardId);
        if(rtn != null) return rtn;
        
        if (Adding)
        {
            Input = new KeepActionConditionInputModel();
            return Page();
        }

        var condition = await _cardActionService.FindKeepActionCondition(groupId, conditionId);
        if (condition == null) return new NotFoundResult();

        Input = new KeepActionConditionInputModel(condition);
        return Page();
    }
    
    public async Task<IActionResult> OnPost(int cardId, int groupId, int conditionId)
    {
        SetupAdding(conditionId);
        var rtn = await SetupPage(cardId);
        if(rtn != null) return rtn;
        
        if (!ModelState.IsValid) return Page();

        bool res;
        if (Adding)
        {
            var condition = new KeepActionCondition
            {
                TenantId = _userInfo.TenantId,
                ActionGroupId = groupId
            };
            Input.Fill(condition);
            res = await _cardActionService.AddKeepActionCondition(condition, ModelState);
        }
        else
        {
            var condition = await _cardActionService.FindKeepActionCondition(groupId, conditionId);
            if (condition == null) return new NotFoundResult();
            Input.Fill(condition);
            res = await _cardActionService.UpdateKeepActionCondition(condition, ModelState);
        }

        return res ? RedirectToPage($"/Objects/Cards/Action/{nameof(Action.Index)}", new { cardId }) : Page();
    }
}