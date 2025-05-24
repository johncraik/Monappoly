using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.GameModels.Enums;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Cards.CardActions;

public class KeepActionCondition : DataModel
{
    public int Id { get; set; }
    public int ActionGroupId { get; set; }
    [ForeignKey(nameof(ActionGroupId))]
    public virtual ActionGroup ActionGroup { get; set; }
    
    public bool IsUntilNeeded { get; set; }
    public CardPlayCondition PlayCondition { get; set; } = CardPlayCondition.None;
    public ObjectPlayer PlayerCondition { get; set; }

    public ActionGroupLengthType GroupLengthType { get; set; } = ActionGroupLengthType.None;
    public uint? LengthValue { get; set; }
}

public enum CardPlayCondition
{
    None = -1,
    Double,
    Triple,
    Go,
    InJail,
    FreeParking,
    GoToJail
}

public enum ActionGroupLengthType
{
    None = -1,
    Turns,
    Jail,
    Double,
    Triple,
}