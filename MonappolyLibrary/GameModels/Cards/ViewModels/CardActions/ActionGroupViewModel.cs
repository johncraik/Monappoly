using System.ComponentModel;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

public class ActionGroupViewModel
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public List<KeepActionCondition> KeepActionConditions { get; set; } = [];

    public ActionGroupViewModel()
    {
    }
    
    public ActionGroupViewModel(int cardId)
    {
        CardId = cardId;
    }

    public ActionGroupViewModel(ActionGroup group, List<KeepActionCondition> keepActionConditions)
    {
        Id = group.Id;
        CardId = group.CardId;
        KeepActionConditions = keepActionConditions;
    }
}