using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.TakeCard;

public class SingleCardAction : ICardAction, ITakeCardAction
{
    public ActionType Type { get; set; } = ActionType.TakeCard;
    public TakeCardActionType TakeCardType { get; set; } = TakeCardActionType.SingleCard;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    [DisplayName("Card Type")]
    public int CardTypeId { get; set; }
    public string? CardTypeName { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (Type != ActionType.TakeCard || TakeCardType != TakeCardActionType.SingleCard)
        {
            throw new InvalidOperationException("Invalid ActionType or TakeCardActionType.");
        }

        if (CardTypeId <= 0)
        {
            modelState.AddModelError(nameof(CardTypeId), "You must select a card type.");
        }

        TurnLength = 0;
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Card Type:", CardTypeName ?? "Unknown", null)
        };
        
        return new ActionViewModel(this, props);
    }
}
