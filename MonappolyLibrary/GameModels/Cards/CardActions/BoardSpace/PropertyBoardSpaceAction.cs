using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;

public class PropertyBoardSpaceAction : ICardAction, IBoardSpaceAction
{
    public ActionType Type { get; set; } = ActionType.BoardSpace;
    public BoardSpaceActionType BoardSpaceType { get; set; } = BoardSpaceActionType.Property;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    public ObjectTarget PropertyOwnership { get; set; }
    public ObjectMultiplier? RentMultiplier { get; set; }
    public uint CustomRent { get; set; }
    public uint MultiplierAmount { get; set; }
    
    public bool IsSet { get; set; }
    public uint SetHouseCount { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.BoardSpace || BoardSpaceType != BoardSpaceActionType.Property)
        {
            throw new InvalidOperationException("Invalid ActionType or BoardSpaceActionType.");
        }

        if (PropertyOwnership == ObjectTarget.Bank)
        {
            RentMultiplier = null;
            CustomRent = 0;
            MultiplierAmount = 0;
        }
        else
        {
            IsSet = false;
            SetHouseCount = 0;
        }

        switch (RentMultiplier)
        {
            case ObjectMultiplier.Fixed when CustomRent == 0:
                modelState.AddModelError(nameof(CustomRent), "Custom Rent must be set when RentMultiplier is Fixed.");
                break;
            case ObjectMultiplier.Custom when MultiplierAmount == 0:
                modelState.AddModelError(nameof(MultiplierAmount), "Multiplier Amount must be set when RentMultiplier is Custom.");
                break;
        }
    }
}