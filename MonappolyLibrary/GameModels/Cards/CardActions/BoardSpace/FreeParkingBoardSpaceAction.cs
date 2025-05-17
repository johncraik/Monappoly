using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;

public class FreeParkingBoardSpaceAction : ICardAction, IBoardSpaceAction
{
    public ActionType Type { get; set; } = ActionType.BoardSpace;
    public BoardSpaceActionType BoardSpaceType { get; set; } = BoardSpaceActionType.FreeParking;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    //Take the money from free parking OR pay into free parking:
    public bool IsTakeMoney { get; set; } = true;
    //Fixed amount to pay in/take out:
    public uint PayInAmount { get; set; } = 0;
    
    //Take properties from free parking OR hand in properties:
    public bool IsTakeProperty { get; set; } = true;
    //Hand in from taking properties out of free parking:
    public uint HandInPropertyCount { get; set; } = 0;
    //Do hand in restrictions apply?
    public bool HandInRestrictions{ get; set; } = true;
    
    //Action types (default, OR multiple properties, OR money multiplied, OR both multiple properties and money multiplied):
    public FreeParkingActionType[] FreeParkingTypes { get; set; } = [FreeParkingActionType.Default];
    //How many properties to hand in/take:
    public uint PropertyMultiplier { get; set; }
    //Amount of money taken/paid in multiplied by:
    public ObjectMultiplier MoneyMultiplier { get; set; }
    //Money multiplier amount if custom multiplier:
    public uint MultiplierAmount { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.BoardSpace || BoardSpaceType != BoardSpaceActionType.FreeParking)
        {
            throw new InvalidOperationException("Invalid ActionType or BoardSpaceActionType.");
        }

        if (FreeParkingTypes.Contains(FreeParkingActionType.Default))
        {
            FreeParkingTypes = [FreeParkingActionType.Default];
        }
        else switch (FreeParkingTypes.Length)
        {
            case 0:
                modelState.AddModelError(nameof(FreeParkingTypes), "At least one FreeParkingActionType must be selected.");
                break;
            case > 1:
                FreeParkingTypes = [FreeParkingActionType.MoneyMultiplied, FreeParkingActionType.MultipleProperties];
                break;
            case 1:
                if (FreeParkingTypes.Contains(FreeParkingActionType.MultipleProperties))
                {
                    FreeParkingTypes = [FreeParkingActionType.MultipleProperties];
                }
                else
                {
                    FreeParkingTypes = [FreeParkingActionType.MoneyMultiplied];
                }
                break;
        }

        if (FreeParkingTypes.Contains(FreeParkingActionType.MultipleProperties) && PropertyMultiplier is 0 or > 28)
        {
            modelState.AddModelError(nameof(PropertyMultiplier), "PropertyMultiplier must be between 1 and 28.");
        }

        if (FreeParkingTypes.Contains(FreeParkingActionType.MoneyMultiplied) &&
            MoneyMultiplier == ObjectMultiplier.Fixed && PayInAmount == 0)
        {
            modelState.AddModelError(nameof(PayInAmount), "PayInAmount must be greater than 0.");
        }

        if (FreeParkingTypes.Contains(FreeParkingActionType.MoneyMultiplied) &&
            MoneyMultiplier == ObjectMultiplier.Custom && MultiplierAmount == 0)
        {
            modelState.AddModelError(nameof(MultiplierAmount), "MultiplierAmount must be greater than 0.");
        }

        if (!IsTakeProperty)
        {
            //This property refers to handing in when taking:
            HandInPropertyCount = 0;
        }
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Free Parking Money:", IsTakeMoney ? "Take Money" : "Pay in Money", null),
            ("Money Amount/Cap:", PayInAmount.ToString(), null),
            ("Free Parking Properties:", IsTakeProperty ? "Take Properties" : "Hand In Properties", null),
            ("Hand In Properties:", HandInPropertyCount.ToString(), !IsTakeProperty),
            ("Hand In Restrictions:", HandInRestrictions ? "Yes" : "No", !IsTakeProperty),
            ("Fixed Property Multiplier:", PropertyMultiplier.ToString(), FreeParkingTypes.Contains(FreeParkingActionType.MoneyMultiplied) && !FreeParkingTypes.Contains(FreeParkingActionType.Default)),
            ("Money Multiplier:", MoneyMultiplier.GetDisplayName(), FreeParkingTypes.Contains(FreeParkingActionType.MultipleProperties) && !FreeParkingTypes.Contains(FreeParkingActionType.Default)),
            ("Multiplier Amount:", MultiplierAmount.ToString(), FreeParkingTypes.Contains(FreeParkingActionType.MultipleProperties) && !FreeParkingTypes.Contains(FreeParkingActionType.Default))
        };
        
        return new ActionViewModel(this, props);
    }
}

public enum FreeParkingActionType
{
    Default = -1,
    MultipleProperties,
    MoneyMultiplied
}