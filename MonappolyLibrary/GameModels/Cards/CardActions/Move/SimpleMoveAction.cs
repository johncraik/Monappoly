using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public class SimpleMoveAction : ICardAction, IMoveAction
{
    public ActionType Type { get; set; } = ActionType.Move;
    public MoveActionType MoveType { get; set; } = MoveActionType.Simple;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    [DisplayName("Spaces")]
    public uint Value { get; set; }
    public bool IsBackwards { get; set; } = false;
    [DisplayName("Advance to Space?")]
    public bool IsAdvance { get; set; } = false;
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(MoveType != MoveActionType.Simple || Type != ActionType.Move)
        {
            throw new InvalidOperationException("Invalid ActionType or MoveActionType");
        }
        
        if (Value == 0 && !IsAdvance)
        {
            modelState.AddModelError(nameof(Value), "Value must be greater than 0");
        }

        TurnLength = 0;
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Spaces:", Value.ToString(), null),
            ("Move Backwards:", IsBackwards.ToString(), null),
            ("Advance to Space:", IsAdvance.ToString(), null)
        };
        
        return new ActionViewModel(this, props);
    }
}