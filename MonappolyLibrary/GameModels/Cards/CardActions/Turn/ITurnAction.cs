using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Turn;

public interface ITurnAction
{
    TurnActionType TurnType { get; set; }
    ObjectPlayer Player { get; set; }
}

public enum TurnActionType
{
    Miss,
    Extra
}