using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Cards;

public class Card : DataModel
{
    public int Id { get; set; }
    public required string Text { get; set; }
    
    public int CardTypeId { get; set; }
    [ForeignKey(nameof(CardTypeId))]
    public virtual CardType CardType { get; set; }
    
    public int CardDeckId { get; set; }
    [ForeignKey(nameof(CardDeckId))]
    public virtual CardDeck CardDeck { get; set; }
    
    public Card CopyCard(int deckId)
    {
        var newCard = new Card
        {
            Text = Text,
            CardTypeId = CardTypeId,
            CardDeckId = deckId
        };
        return newCard;
    }
}