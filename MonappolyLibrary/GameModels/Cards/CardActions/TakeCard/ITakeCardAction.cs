using System.ComponentModel;

namespace MonappolyLibrary.GameModels.Cards.CardActions.TakeCard;

public interface ITakeCardAction
{
    [DisplayName("Take Card Type")]
    TakeCardActionType TakeCardType { get; set; }
}

public class BaseTakeCardAction : ITakeCardAction
{
    public TakeCardActionType TakeCardType { get; set; }
}

public enum TakeCardActionType
{
    SingleCard
}