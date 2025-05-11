using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Money;

public class PayMoneyAction : ICardAction, IMoneyAction
{
    public ActionType Type { get; set; } = ActionType.Money;
    public MoneyActionType MoneyType { get; set; } = MoneyActionType.Pay;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    [DisplayName("Pay Amount")]
    public int Value { get; set; }
    [DisplayName("Payment Multiplier")]
    public ObjectMultiplier MoneyMultiplier { get; set; }
    
    public ObjectTarget Source { get; set; }
    public ObjectPlayer? SourcePlayer { get; set; } = null;
    
    public ObjectTarget Target { get; set; }
    public ObjectPlayer? TargetPlayer { get; set; } = null;
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Money || MoneyType != MoneyActionType.Pay)
        {
            throw new InvalidOperationException("Invalid ActionType or MoneyActionType");
        }
        
        if(Source == ObjectTarget.Player && SourcePlayer == null)
        {
            modelState.AddModelError("SourcePlayer", "Source player must be specified when source is a player.");
        }
        
        if(Target == ObjectTarget.Player && TargetPlayer == null)
        {
            modelState.AddModelError("TargetPlayer", "Target player must be specified when target is a player.");
        }
        
        if (MoneyMultiplier == ObjectMultiplier.Custom)
        {
            modelState.AddModelError("MoneyMultiplier", "Custom multiplier is not supported.");
        }
    }
}