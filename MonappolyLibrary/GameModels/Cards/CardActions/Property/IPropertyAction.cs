using System.ComponentModel;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Property;

public interface IPropertyAction
{
    [DisplayName("Property Type")]
    PropertyActionType PropertyType { get; set; }
    ObjectPlayer Player { get; set; }
    [DisplayName("Property Count")]
    uint PropertyCount { get; set; }
    [DisplayName("Is a Set of Properties?")]
    public bool IsSet { get; set; }
}

public class BasePropertyAction : IPropertyAction
{
    public PropertyActionType PropertyType { get; set; }
    public ObjectPlayer Player { get; set; }
    public uint PropertyCount { get; set; }
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