using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Cards;

public class CardDeck : DataModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    
    public CardDeckDifficulty Difficulty { get; set; } = CardDeckDifficulty.Default;
}

public enum CardDeckDifficulty
{
    Default = -1,
    Easy,
    Medium,
    Hard,
    Extreme
}        