using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Dice;

public class RerollDiceAction : ICardAction, IDiceAction
{
    public ActionType Type { get; set; } = ActionType.Dice;
    public DiceActionType DiceType { get; set; } = DiceActionType.Reroll;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    public byte DiceCount { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Dice || DiceType != DiceActionType.Reroll)
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
            ("Dice Count:", DiceCount.ToString(), null)
        };

        return new ActionViewModel(this, props);
    }
}