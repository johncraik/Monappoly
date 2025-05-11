using System.ComponentModel;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Dice;

public interface IDiceAction
{
    [DisplayName("Dice Type")]
    DiceActionType DiceType { get; set; }
    [DisplayName("Dice Count")]
    byte DiceCount { get; set; }
}

public class BaseDiceAction : IDiceAction
{
    public DiceActionType DiceType { get; set; }
    public byte DiceCount { get; set; }
}

public enum DiceActionType
{
    Convert,
    Downgrade,
    Reroll
}