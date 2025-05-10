using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Move;

public class SpecialMoveAction : ICardAction, IMoveAction
{
    public ActionType Type { get; set; } = ActionType.Move;
    public MoveActionType MoveType { get; set; } = MoveActionType.Special;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    public SpecialMoveActionType SpecialMoveType {get; set;}
    public bool IsBackwards { get; set; } = false;

    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Move || MoveType != MoveActionType.Special)
        {
            throw new InvalidOperationException("Invalid ActionType or MoveActionType");
        }

        TurnLength = 0;
    }
}

public enum SpecialMoveActionType
{
    Property,
    Station,
    Utility,
    Tax,
    Chance,
    CommunityChest
}