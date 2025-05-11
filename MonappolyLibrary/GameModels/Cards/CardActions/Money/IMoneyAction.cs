using System.ComponentModel;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Money;

public interface IMoneyAction
{
    [DisplayName("Money Type")]
    MoneyActionType MoneyType { get; set; }
    int Value { get; set; }
    ObjectMultiplier MoneyMultiplier { get; set; }
    ObjectTarget Source { get; set; }
    [DisplayName("Source Player")]
    ObjectPlayer? SourcePlayer { get; set; }
    ObjectTarget Target { get; set; }
    [DisplayName("Target Player")]
    ObjectPlayer? TargetPlayer { get; set; }
}

public class BaseMoneyAction : IMoneyAction
{
    public MoneyActionType MoneyType { get; set; }
    public int Value { get; set; }
    public ObjectMultiplier MoneyMultiplier { get; set; }
    public ObjectTarget Source { get; set; }
    public ObjectPlayer? SourcePlayer { get; set; }
    public ObjectTarget Target { get; set; }
    public ObjectPlayer? TargetPlayer { get; set; }
}

public enum MoneyActionType
{
    Pay,
    Receive
}

