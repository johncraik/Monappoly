using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonappolyLibrary.GameModels.Cards.CardActions.TakeCard;

public class SingleCardAction : ICardAction, ITakeCardAction
{
    public ActionType Type { get; set; } = ActionType.TakeCard;
    public TakeCardActionType TakeCardType { get; set; } = TakeCardActionType.SingleCard;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public int CardTypeId { get; set; }
    
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
}
