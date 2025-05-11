using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Player;

public class DiceNumberPlayerAction : ICardAction, IPlayerAction
{
    public ActionType Type { get; set; } = ActionType.Player;
    public PlayerActionType PlayerType { get; set; } = PlayerActionType.DiceNumber;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public ObjectPlayer Player { get; set; }
    [DisplayName("Reset Dice Number?")]
    public bool IsReset { get; set; }
    
    [DisplayName("Have Player's Number Rolled?")]
    public bool CallNumber { get; set; } = false;
    [DisplayName("Has Player Rolled the Number?")]
    public bool HasRolled { get; set; } = false;
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Player || PlayerType != PlayerActionType.DiceNumber)
        {
            throw new InvalidOperationException("Invalid ActionType or PlayerActionType.");
        }

        if (IsReset)
        {
            CallNumber = false;
            HasRolled = false;
        }
        else if (!CallNumber)
        {
            modelState.AddModelError(nameof(CallNumber), "CallNumber must be true if IsReset is false.");
        }
        
        TurnLength = 0;
    }
}