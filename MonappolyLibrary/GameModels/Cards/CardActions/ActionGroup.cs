using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.GameModels.Enums;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Cards.CardActions;

public class ActionGroup : DataModel
{
    private const string PlayConditionDelimiter = "/¬/";
    
    public int Id { get; set; }
    
    public int CardId {get; set;}
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
    
    public bool IsKeep { get; set; } = false;
    
    public string WrappedPlayConditions { get; set; } = $"{ActionPlayCondition.Default}{PlayConditionDelimiter}";
    [NotMapped]
    public List<ActionPlayCondition> PlayCondition { get; set; } = [ActionPlayCondition.Default];
    public void UnwrapPlayConditions()
    {
        PlayCondition = WrappedPlayConditions.Split(PlayConditionDelimiter, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => (ActionPlayCondition)Enum.Parse(typeof(ActionPlayCondition), x))
            .ToList();
    }
    public void WrapPlayConditions()
    {
        WrappedPlayConditions = string.Join(PlayConditionDelimiter, PlayCondition);
    }
    
    
    public ActionLengthType GroupLengthType { get; set; } = ActionLengthType.Default;
    public ObjectPlayer Player { get; set; }
    public bool IsForced { get; set; } = false;
    
    [NotMapped]
    public IEnumerable<ICardAction> Actions { get; set; }
}

public enum ActionPlayCondition
{
    Default = -1,
    UntilNeeded,
    Double,
    Triple,
    Go,
    Jail,   //in jail NOT go to
    FreeParking
}

public enum ActionLengthType
{
    Default = -1,
    Turns,
    Jail,
    Double,
    Triple,
}