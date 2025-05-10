using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public class SwapMoveAction : ICardAction, IMoveAction
{
    public ActionType Type { get; set; } = ActionType.Move;
    public MoveActionType MoveType { get; set; } = MoveActionType.Swap;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    public ObjectPlayer SourcePlayer { get; set; }
    public ObjectPlayer TargetPlayer { get; set; }
    
    public bool IsDoSpaceAction { get; set; } = true;
    public bool IsBackwards { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Move || MoveType != MoveActionType.Swap)
        {
            throw new InvalidOperationException("Invalid ActionType or MoveActionType.");
        }

        if (SourcePlayer == ObjectPlayer.All)
        {
            modelState.AddModelError("SourcePlayer", "SourcePlayer cannot be 'All'.");
        }
        
        if (TargetPlayer == ObjectPlayer.All)
        {
            modelState.AddModelError("TargetPlayer", "TargetPlayer cannot be 'All'.");
        }

        TurnLength = 0;
    }
}