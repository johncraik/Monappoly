using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public class SwapMoveAction : ICardAction, IMoveAction
{
    public ActionType Type { get; set; } = ActionType.Move;
    public MoveActionType MoveType { get; set; } = MoveActionType.Swap;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    [DisplayName("Source Player")]
    public ObjectPlayer SourcePlayer { get; set; }
    [DisplayName("Target Player")]
    public ObjectPlayer TargetPlayer { get; set; }
    
    [DisplayName("Do Space Action when Swapped?")]
    public bool IsDoSpaceAction { get; set; } = true;
    [DisplayName("Move Backwards?")]
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
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Source Player:", SourcePlayer.GetDisplayName(), null),
            ("Target Player:", TargetPlayer.GetDisplayName(), null),
            ("Do Space Action when Swapped?", IsDoSpaceAction.ToString(), null),
            ("Move Backwards?", IsBackwards.ToString(), null)
        };
        
        return new ActionViewModel(this, props);
    }
}