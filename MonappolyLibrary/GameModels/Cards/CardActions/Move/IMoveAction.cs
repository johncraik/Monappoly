using System.ComponentModel;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public interface IMoveAction
{
    [DisplayName("Move Type")]
    MoveActionType MoveType { get; set; }
    [DisplayName("Move Backwards?")]
    bool IsBackwards { get; set; }
}

public class BaseMoveAction : IMoveAction
{
    public MoveActionType MoveType { get; set; }
    public bool IsBackwards { get; set; }
}

public enum MoveActionType
{
    Simple,
    Special,
    Swap
}