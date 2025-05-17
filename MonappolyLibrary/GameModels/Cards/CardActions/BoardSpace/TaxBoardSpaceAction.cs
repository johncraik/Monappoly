using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;

public class TaxBoardSpaceAction : ICardAction, IBoardSpaceAction
{
    public ActionType Type { get; set; } = ActionType.BoardSpace;
    public BoardSpaceActionType BoardSpaceType { get; set; } = BoardSpaceActionType.Tax;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    public ObjectMultiplier TaxMultiplier { get; set; }
    public uint FixedTax {get; set; }
    public uint CustomMultiplier { get; set; }
    
    public ObjectPlayer Player { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (Type != ActionType.BoardSpace || BoardSpaceType != BoardSpaceActionType.Tax)
        {
            throw new InvalidOperationException("Invalid ActionType or BoardSpaceActionType.");
        }

        switch (TaxMultiplier)
        {
            case ObjectMultiplier.Fixed when FixedTax == 0:
                modelState.AddModelError(nameof(FixedTax), "Fixed tax must be greater than 0.");
                break;
            case ObjectMultiplier.Custom when CustomMultiplier == 0:
                modelState.AddModelError(nameof(CustomMultiplier), "Custom multiplier must be greater than 0.");
                break;
        }
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Tax Multiplier:", TaxMultiplier.GetDisplayName(), null),
            ("Fixed Tax Amount:", FixedTax.ToString(), TaxMultiplier == ObjectMultiplier.Fixed),
            ("Custom Multiplier Amount:", CustomMultiplier.ToString(), TaxMultiplier == ObjectMultiplier.Custom),
            ("Target Player:", Player.GetDisplayName(), null)
        };
        
        return new ActionViewModel(this, props);
    }
}