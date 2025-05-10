namespace MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;

public interface IBoardSpaceAction
{
    BoardSpaceActionType BoardSpaceType { get; set; }
}

public enum BoardSpaceActionType
{
    Go,
    Jail,
    FreeParking,
    Tax,
    Card,
    Property
}