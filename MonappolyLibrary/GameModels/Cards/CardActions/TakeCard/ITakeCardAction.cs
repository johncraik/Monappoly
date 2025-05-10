namespace MonappolyLibrary.GameModels.Cards.CardActions.TakeCard;

public interface ITakeCardAction
{
    TakeCardActionType TakeCardType { get; set; }
}

public enum TakeCardActionType
{
    SingleCard
}