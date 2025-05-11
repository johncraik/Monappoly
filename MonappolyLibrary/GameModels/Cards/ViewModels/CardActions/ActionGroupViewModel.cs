using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

public class ActionGroupViewModel
{
    public int Id { get; set; }
    public int CardId { get; set; }
    
    public bool IsKeep { get; set; }
    public ActionPlayCondition PlayCondition { get; set; }
    public ActionLengthType GroupLengthType { get; set; }
    public ObjectPlayer Player { get; set; }
    public bool IsForced { get; set; }

    public ActionGroupViewModel()
    {
    }

    public ActionGroupViewModel(ActionGroup group)
    {
        Id = group.Id;
        CardId = group.CardId;
        IsKeep = group.IsKeep;
        PlayCondition = group.PlayCondition;
        GroupLengthType = group.GroupLengthType;
        Player = group.Player;
        IsForced = group.IsForced;
    }
    
    public void Fill(ActionGroup group)
    {
        group.Id = Id;
        group.CardId = CardId;
        group.IsKeep = IsKeep;
        group.PlayCondition = PlayCondition;
        group.GroupLengthType = GroupLengthType;
        group.Player = Player;
        group.IsForced = IsForced;
    }
}