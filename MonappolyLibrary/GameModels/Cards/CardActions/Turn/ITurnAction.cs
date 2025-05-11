using System.ComponentModel;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Turn;

public interface ITurnAction
{
    [DisplayName("Turn Type")]
    TurnActionType TurnType { get; set; }
    ObjectPlayer Player { get; set; }
}

public class BaseTurnAction : ITurnAction
{
    public TurnActionType TurnType { get; set; }
    public ObjectPlayer Player { get; set; }
}

public enum TurnActionType
{
    Miss,
    Extra
}