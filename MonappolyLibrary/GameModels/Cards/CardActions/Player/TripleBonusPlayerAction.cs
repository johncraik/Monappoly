using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Player;

public class TripleBonusPlayerAction : ICardAction, IPlayerAction
{
    public ActionType Type { get; set; } = ActionType.Player;
    public PlayerActionType PlayerType { get; set; } = PlayerActionType.TripleBonus;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public ObjectPlayer Player { get; set; }
    [DisplayName("Reset Triple Bonus?")]
    public bool IsReset { get; set; }

    [DisplayName("Increase Triple Bonus?")]
    public bool IsIncreased { get; set; } = true;
    [DisplayName("Triple Bonus Multiplier")]
    public ObjectMultiplier? CostMultiplier {get; set;}
    [DisplayName("Fixed Triple Bonus")]
    public uint FixedCost { get; set; }
    [DisplayName("Custom Triple Bonus Multiplier")]
    public uint CustomMultiplier { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Player || PlayerType != PlayerActionType.TripleBonus)
        {
            throw new InvalidOperationException("Invalid ActionType or PlayerActionType.");
        }

        if (IsReset)
        {
            CostMultiplier = null;
            FixedCost = 0;
            CustomMultiplier = 0;
            IsIncreased = false;
        }

        switch (CostMultiplier)
        {
            case ObjectMultiplier.Fixed when FixedCost == 0:
                modelState.AddModelError(nameof(FixedCost), "Fixed cost must be greater than 0.");
                break;
            case ObjectMultiplier.Custom when CustomMultiplier == 0:
                modelState.AddModelError(nameof(CustomMultiplier), "Custom multiplier must be greater than 0.");
                break;
        }

        TurnLength = 0;
    }
}