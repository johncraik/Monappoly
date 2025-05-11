using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Property;

public class MortgagePropertyAction : ICardAction, IPropertyAction
{
    public ActionType Type { get; set; } = ActionType.Property;
    public PropertyActionType PropertyType { get; set; } = PropertyActionType.Mortgage;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public ObjectPlayer Player { get; set; }
    public uint PropertyCount { get; set; }
    public bool IsSet { get; set; }
    [DisplayName("Receive Mortgage Value?")]
    public bool IsReceiveValue { get; set; } = true;
    [DisplayName("Pay Mortgage Penalty?")]
    public bool PayPenalty { get; set; } = true;

    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Property || PropertyType != PropertyActionType.Mortgage)
        {
            throw new InvalidOperationException("Invalid ActionType or PropertyActionType.");
        }
        
        switch (PropertyCount)
        {
            case 0:
                modelState.AddModelError(nameof(PropertyCount), "Property count must be greater than 0.");
                break;
            case > 28:
                modelState.AddModelError(nameof(PropertyCount), "Property count must be less than or equal to 28.");
                break;
        }

        if (IsSet && PropertyCount > 10)
        {
            modelState.AddModelError(nameof(PropertyCount), "Property count must be less than or equal to 10 when IsSet is true.");
        }

        if (TurnLength == 0)
        {
            modelState.AddModelError(nameof(TurnLength), "Turn length must be greater than 0.");
        }
    }
}