using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;

public class GoBoardSpaceAction : ICardAction, IBoardSpaceAction
{
    //Action relates to the money you get when landing on GO
    public ActionType Type { get; set; } = ActionType.BoardSpace;
    public BoardSpaceActionType BoardSpaceType { get; set; } = BoardSpaceActionType.Go;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public bool IsMultiplied { get; set; } = true;
    public ObjectMultiplier? Multiplier { get; set; }
    public uint MultiplierAmount { get; set; }
    public ObjectPlayer? TargetPlayer { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.BoardSpace || BoardSpaceType != BoardSpaceActionType.Go)
        {
            throw new InvalidOperationException("Invalid ActionType or BoardSpaceActionType.");
        }

        switch (IsMultiplied)
        {
            case true when Multiplier == null:
                modelState.AddModelError(nameof(Multiplier), "Multiplier is required when IsMultiplied is true.");
                break;
            case false when TargetPlayer == null:
                modelState.AddModelError(nameof(TargetPlayer), "TargetPlayer is required when IsMultiplied is false.");
                break;
        }
        
        if (Multiplier == ObjectMultiplier.Custom && MultiplierAmount == 0)
        {
            modelState.AddModelError(nameof(MultiplierAmount), "MultiplierAmount is required when Multiplier is Custom.");
        }
    }

    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("GO Money Multiplied?", IsMultiplied ? "Yes" : "No", null),
            ("Money Multiplier:", Multiplier?.GetDisplayName() ?? "", IsMultiplied),
            ("Custom Multiplier Amount:", MultiplierAmount.ToString(), IsMultiplied),
            ("Target Player for Money:", TargetPlayer?.GetDisplayName() ?? "", !IsMultiplied)
        };
        
        return new ActionViewModel(this, props);
    }
}