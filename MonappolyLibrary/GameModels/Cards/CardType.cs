using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Cards;

public class CardType : DataModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    public string? Colour { get; set; }
    public CardTypeCondition Condition { get; set; }
}

public enum CardTypeCondition
{
    Default,
    Chance,
    CommunityChest,
    Double,
    Triple,
    Tax,
    FreeParking,
    Jail,
    Go,
    Other = -1
}