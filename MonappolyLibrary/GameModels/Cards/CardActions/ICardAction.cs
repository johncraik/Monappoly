namespace MonappolyLibrary.GameModels.Cards.CardActions;

public interface ICardAction
{
    int Id {get; set;}
    int GroupId { get; set; }
    ActionType Type { get; }
    uint TurnLength { get; set; }
}

public enum ActionType
{
    Move,
    Dice,
    Turn,
    Money,
    Player,
    Property,
    BoardSpace,
    TakeCard
}