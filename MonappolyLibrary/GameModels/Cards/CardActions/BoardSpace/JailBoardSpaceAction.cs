using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;

public class JailBoardSpaceAction : ICardAction, IBoardSpaceAction
{
    public ActionType Type { get; set; } = ActionType.BoardSpace;
    public BoardSpaceActionType BoardSpaceType { get; set; } = BoardSpaceActionType.Jail;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    public bool CanReceiveRent { get; set; } = true;
    public bool MustStay { get; set; } 
    public bool LeaveJail { get; set; } 
    
    public ObjectPlayer TargetPlayer { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.BoardSpace || BoardSpaceType != BoardSpaceActionType.Jail)
        {
            throw new InvalidOperationException("Invalid ActionType or BoardSpaceActionType.");
        }

        if (LeaveJail)
        {
            MustStay = false;
            CanReceiveRent = false;
        }
        
        switch (MustStay)
        {
            case true when TurnLength == 0:
                modelState.AddModelError("TurnLength", "TurnLength must be greater than 0 if MustStay is true.");
                break;
            case false:
                TurnLength = 0;
                break;
        }
    }
    
    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Can Receive Rent?", CanReceiveRent ? "Yes" : "No", null),
            ("Must Stay in Jail?", MustStay ? "Yes" : "No", null),
            ("Get out of Jail Free?", LeaveJail ? "Yes" : "No", null),
            ("Target Player:", TargetPlayer.GetDisplayName(), null)
        };
        
        return new ActionViewModel(this, props);
    }
}