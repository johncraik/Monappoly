using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public class SpecialMoveAction : ICardAction, IMoveAction
{
    public ActionType Type { get; set; } = ActionType.Move;
    public MoveActionType MoveType { get; set; } = MoveActionType.Special;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    [DisplayName("Advance to Nearest")]
    public SpecialMoveActionType SpecialMoveType {get; set;}
    [DisplayName("Is Owned by a Player?")]
    public bool OwnedSpace { get; set; } = false;
    public bool IsBackwards { get; set; } = false;

    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Move || MoveType != MoveActionType.Special)
        {
            throw new InvalidOperationException("Invalid ActionType or MoveActionType");
        }

        TurnLength = 0;
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Advance to Nearest:", SpecialMoveType.GetDisplayName(), null),
            ("Owned Space?", OwnedSpace.ToString(), null),
            ("Move Backwards?", IsBackwards.ToString(), null)
        };
        
        return new ActionViewModel(this, props);
    }
}

public enum SpecialMoveActionType
{
    Property,
    Station,
    Utility,
    Tax,
    Chance,
    CommunityChest
}