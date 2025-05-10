using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Property;

public class ReturnPropertyAction : ICardAction, IPropertyAction
{
    public ActionType Type { get; set; } = ActionType.Property;
    public PropertyActionType PropertyType { get; set; } = PropertyActionType.Return;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public uint PropertyCount { get; set; }
    public bool IsSet { get; set; }
    
    public ObjectPlayer Player { get; set; }
    public ObjectTarget Target { get; set; }
    public ObjectPlayer? TargetPlayer { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Property || PropertyType != PropertyActionType.Return)
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
        
        if(Target == ObjectTarget.Player && TargetPlayer == null)
        {
            modelState.AddModelError(nameof(TargetPlayer), "Target player must be specified when target is a player.");
        }
    }
}