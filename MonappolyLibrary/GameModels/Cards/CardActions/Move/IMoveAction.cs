namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public interface IMoveAction
{
    MoveActionType MoveType { get; set; }
}

public enum MoveActionType
{
    Simple,
    Special,
    Swap
}