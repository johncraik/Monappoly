using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Dice;

public class DowngradeDiceAction : ICardAction, IDiceAction
{
    public ActionType Type { get; set; } = ActionType.Dice;
    public DiceActionType DiceType { get; set; } = DiceActionType.Downgrade;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public byte DiceCount { get; set; }
    [DisplayName("Downgrade a Triple to a Double?")]
    public bool ToDouble { get; set; } = true;
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Dice || DiceType != DiceActionType.Downgrade)
        {
            throw new InvalidOperationException("Invalid ActionType or DiceActionType.");
        }
        
        if(DiceCount > 3)
        {
            modelState.AddModelError(nameof(DiceCount), "Dice count must be 1, 2, or 3.");
        }
    }

    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Dice Count:", DiceCount.ToString(), null),
            ("Downgrade to Double:", ToDouble.ToString(), null)
        };

        return new ActionViewModel(this, props);
    }
}