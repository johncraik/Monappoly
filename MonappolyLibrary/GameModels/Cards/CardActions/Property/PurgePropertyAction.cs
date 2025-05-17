using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Property;

public class PurgePropertyAction : ICardAction, IPropertyAction
{
    public ActionType Type { get; set; } = ActionType.Property;
    public PropertyActionType PropertyType { get; set; } = PropertyActionType.Purge;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public ObjectPlayer Player { get; set; }
    public uint PropertyCount { get; set; }
    public bool IsSet { get; set; }

    public void Validate(ModelStateDictionary modelState)
    {
        if (Type != ActionType.Property || PropertyType != PropertyActionType.Purge)
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
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Player:", Player.GetDisplayName(), null),
            ("Property Count:", PropertyCount.ToString(), null),
            ("Is Set:", IsSet ? "Yes" : "No", null)
        };
        
        return new ActionViewModel(this, props);
    }
}