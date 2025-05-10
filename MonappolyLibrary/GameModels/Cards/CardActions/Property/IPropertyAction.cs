using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Property;

public interface IPropertyAction
{
    PropertyActionType PropertyType { get; set; }
    ObjectPlayer Player { get; set; }
    uint PropertyCount { get; set; }
    public bool IsSet { get; set; }
}

public enum PropertyActionType
{
    Take,
    Return,
    Mortgage,
    Unmortgage,
    Purge
}