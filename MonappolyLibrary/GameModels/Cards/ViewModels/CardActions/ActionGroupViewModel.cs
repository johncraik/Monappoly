using System.ComponentModel;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

public class ActionGroupViewModel
{
    public int Id { get; set; }
    public int CardId { get; set; }
    
    [DisplayName("Player Keeps Card?")]
    public bool IsKeep { get; set; }
    [DisplayName("Play Conditions")]
    public List<ActionPlayCondition> PlayCondition { get; set; }
    [DisplayName("Group Length Condition")]
    public ActionLengthType GroupLengthType { get; set; }
    public ObjectPlayer Player { get; set; }
    [DisplayName("Player is Forced to Play Group when Conditions Met?")]
    public bool IsForced { get; set; }

    public ActionGroupViewModel()
    {
    }
    
    public ActionGroupViewModel(int cardId)
    {
        CardId = cardId;
    }

    public ActionGroupViewModel(ActionGroup group)
    {
        Id = group.Id;
        CardId = group.CardId;
        IsKeep = group.IsKeep;
        
        group.UnwrapPlayConditions();
        PlayCondition = group.PlayCondition;
        
        GroupLengthType = group.GroupLengthType;
        Player = group.Player;
        IsForced = group.IsForced;
    }
    
    public void Fill(ActionGroup group)
    {
        group.Id = Id;
        group.IsKeep = IsKeep;
        group.CardId = CardId;
        
        group.PlayCondition = PlayCondition;
        group.WrapPlayConditions();
        
        group.GroupLengthType = GroupLengthType;
        group.Player = Player;
        group.IsForced = IsForced;
    }
}