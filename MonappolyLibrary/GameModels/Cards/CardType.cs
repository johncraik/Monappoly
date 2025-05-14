using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Cards;

public class CardType : DataModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    public string? Colour { get; set; }
    public CardTypeCondition PlayCondition { get; set; }
    public CardTypeRule TypeRule { get; set; }
}

public enum CardTypeCondition
{
    Default,
    WithChance,
    WithCommunityChest,
    Double,
    Triple,
    Tax,
    FreeParking,
    Jail,
    Go,
    Other = -1
}

public enum CardTypeRule
{
    Default,
    PercentageRule
}