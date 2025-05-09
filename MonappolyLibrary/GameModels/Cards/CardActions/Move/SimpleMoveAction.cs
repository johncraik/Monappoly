namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public class SimpleMoveAction : ICardAction
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public ActionType Type { get; }
    public uint TurnLength { get; set; }
}