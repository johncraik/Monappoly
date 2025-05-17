using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Money;

public class ReceiveMoneyAction : ICardAction, IMoneyAction
{
    public ActionType Type { get; set; } = ActionType.Money;
    public MoneyActionType MoneyType { get; set; } = MoneyActionType.Receive;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    [DisplayName("Receive Amount")]
    public int Value { get; set; }
    [DisplayName("Received Amount Multiplier")]
    public ObjectMultiplier MoneyMultiplier { get; set; }
    
    public ObjectTarget Source { get; set; }
    public ObjectPlayer? SourcePlayer { get; set; }
    
    public ObjectTarget Target { get; set; }
    public ObjectPlayer? TargetPlayer { get; set; }

    public void Validate(ModelStateDictionary modelState)
    {
        if (Type != ActionType.Money || MoneyType != MoneyActionType.Receive)
        {
            throw new InvalidOperationException("Invalid ActionType or MoneyActionType.");
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
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Receive Amount:", Value.ToString(), null),
            ("Source:", Source.GetDisplayName(), null),
            ("Source Player:", SourcePlayer?.GetDisplayName() ?? "", Source == ObjectTarget.Player),
            ("Target:", Target.GetDisplayName(), null),
            ("Target Player:", TargetPlayer?.GetDisplayName() ?? "", Target == ObjectTarget.Player),
            ("Money Multiplier:", MoneyMultiplier.GetDisplayName(), null)
        };
        
        return new ActionViewModel(this, props);
    }
}