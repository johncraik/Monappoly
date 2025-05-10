using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Player;

public interface IPlayerAction
{
    PlayerActionType PlayerType { get; set; }
    ObjectPlayer Player { get; set; }
    bool IsReset { get; set; }
}

public enum PlayerActionType
{
    JailCost,
    TripleBonus,
    DiceNumber
}