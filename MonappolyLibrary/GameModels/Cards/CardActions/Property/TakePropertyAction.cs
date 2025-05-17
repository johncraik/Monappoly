using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Property;

public class TakePropertyAction : ICardAction, IPropertyAction
{
    public ActionType Type { get; set; } = ActionType.Property;
    public PropertyActionType PropertyType { get; set; } = PropertyActionType.Take;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    [DisplayName("Number of Turns")]
    public uint TurnLength { get; set; }
    
    [DisplayName("Number of Properties")]
    public uint PropertyCount { get; set; }
    [DisplayName("Take a Set?")]
    public bool IsSet { get; set; }
    
    public ObjectPlayer Player { get; set; }
    public ObjectTarget Source { get; set; }
    [DisplayName("Source Player")]
    public ObjectPlayer? SourcePlayer { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Property || PropertyType != PropertyActionType.Take)
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
        
        if(Source == ObjectTarget.Player && SourcePlayer == null)
        {
            modelState.AddModelError(nameof(SourcePlayer), "Target player must be specified when target is a player.");
        }
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Property Count:", PropertyCount.ToString(), null),
            ("Take a Set:", IsSet.ToString(), null),
            ("Player:", Player.GetDisplayName(), null),
            ("Source:", Source.GetDisplayName(), null),
            ("Source Player:", SourcePlayer?.GetDisplayName() ?? "", Source == ObjectTarget.Player)
        };
        
        return new ActionViewModel(this, props);
    }
}