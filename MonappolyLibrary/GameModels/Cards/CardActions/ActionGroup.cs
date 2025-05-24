using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.GameModels.Enums;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Cards.CardActions;

public class ActionGroup : DataModel
{
    public int Id { get; set; }
    public int NextActionId { get; set; }
    
    public int CardId {get; set;}
    [ForeignKey(nameof(CardId))]
    public virtual Card Card { get; set; }
    
    [NotMapped]
    public IEnumerable<ICardAction> Actions { get; set; }
}