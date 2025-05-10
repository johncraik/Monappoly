namespace MonappolyLibrary.GameModels.Cards.CardActions.Dice;

public interface IDiceAction
{
    DiceActionType DiceType { get; set; }
    byte DiceCount { get; set; }
}

public enum DiceActionType
{
    Convert,
    Downgrade,
    Reroll
}