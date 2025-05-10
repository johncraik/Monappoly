using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Money;

public interface IMoneyAction
{
    MoneyActionType MoneyType { get; set; }
    int Value { get; set; }
    ObjectMultiplier MoneyMultiplier { get; set; }
    ObjectTarget Source { get; set; }
    ObjectPlayer? SourcePlayer { get; set; }
    ObjectTarget Target { get; set; }
    ObjectPlayer? TargetPlayer { get; set; }
}

public enum MoneyActionType
{
    Pay,
    Receive
}

