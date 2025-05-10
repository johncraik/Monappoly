using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonappolyLibrary.GameModels.Cards.CardActions;

public interface ICardAction
{
    int Id {get; set;}
    int GroupId { get; set; }
    ActionType Type { get; set; }
    uint TurnLength { get; set; }

    void Validate(ModelStateDictionary modelState);
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