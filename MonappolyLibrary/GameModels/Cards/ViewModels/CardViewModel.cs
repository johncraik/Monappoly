namespace MonappolyLibrary.GameModels.Cards.ViewModels;

public class CardViewModel
{
    public int Id { get; set; }
    public string Text { get; set; }
    public CardType? CardType { get; set; }
    public CardDeck? CardDeck { get; set; }
    
    public bool HasActionSetup { get; set; }

    public CardViewModel()
    {
    }
    
    public CardViewModel(Card card, bool? hasAction = null)
    {
        Id = card.Id;
        Text = card.Text;
        CardType = card.CardType;
        CardDeck = card.CardDeck;
        HasActionSetup = hasAction ?? default;
    }

    public void Fill(Card card)
    {
        card.Text = Text;
    }
}