using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

namespace MonappolyLibrary.GameModels.Cards.CardActions;

public interface ICardAction
{
    int Id {get; set;}
    int GroupId { get; set; }
    ActionType Type { get; set; }
    uint TurnLength { get; set; }

    void Validate(ModelStateDictionary modelState);
    ActionViewModel ToViewModel();
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