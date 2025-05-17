using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;

public class CardBoardSpaceAction : ICardAction, IBoardSpaceAction
{
    public ActionType Type { get; set; } = ActionType.BoardSpace;
    public BoardSpaceActionType BoardSpaceType { get; set; } = BoardSpaceActionType.Card;
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }

    public uint CardCount { get; set; } = 1;
    public ObjectPlayer Player { get; set; }
    public bool IsDoubled {get; set;}
    public bool IsTripled {get; set;}
    
    
    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.BoardSpace || BoardSpaceType != BoardSpaceActionType.Card)
        {
           throw new InvalidOperationException("Invalid ActionType or BoardSpaceActionType.");
        }

        if (IsDoubled) IsTripled = false;
        if (IsTripled) IsDoubled = false;
    }

    public ActionViewModel ToViewModel()
    {
        var props = new (string Key, string Value, bool? Condition)[]
        {
            ("Number of cards:", CardCount.ToString(), null),
            ("Player:", Player.GetDisplayName(), null),
            ("Multiplier:", IsDoubled ? "Doubled" : IsTripled ? "Tripled" : "None", null)
        };
        
        return new ActionViewModel(this, props);
    }
}