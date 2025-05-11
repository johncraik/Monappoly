using System.ComponentModel;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Player;

public interface IPlayerAction
{
    [DisplayName("Player Type")]
    PlayerActionType PlayerType { get; set; }
    ObjectPlayer Player { get; set; }
    bool IsReset { get; set; }
}

public class BasePlayerAction : IPlayerAction
{
    public PlayerActionType PlayerType { get; set; }
    public ObjectPlayer Player { get; set; }
    public bool IsReset { get; set; }
}

public enum PlayerActionType
{
    JailCost,
    TripleBonus,
    DiceNumber
}