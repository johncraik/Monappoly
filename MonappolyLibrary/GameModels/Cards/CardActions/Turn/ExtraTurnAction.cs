using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Enums;

namespace MonappolyLibrary.GameModels.Cards.CardActions.Turn;

public class ExtraTurnAction : ICardAction, ITurnAction
{
    public ActionType Type { get; set; } = ActionType.Turn;
    public TurnActionType TurnType { get; set; } = TurnActionType.Miss;

    public int Id { get; set; }
    public int GroupId { get; set; }
    public uint TurnLength { get; set; }
    
    public ObjectPlayer Player { get; set; }
    public bool RollThirdDice { get; set; } = true;

    public void Validate(ModelStateDictionary modelState)
    {
        if(Type != ActionType.Player || TurnType != TurnActionType.Miss)
        {
            throw new InvalidOperationException("Invalid ActionType or TurnActionType.");
        }
        
        if (TurnLength == 0)
        {
            modelState.AddModelError(nameof(TurnLength), "Turn length must be greater than 0.");
        }

        if (Player == ObjectPlayer.All)
        {
            modelState.AddModelError(nameof(Player), "Player cannot be all when having extra turns.");
        }
    }
}