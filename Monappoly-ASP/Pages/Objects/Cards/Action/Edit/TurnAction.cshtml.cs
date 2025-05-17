using Microsoft.AspNetCore.Mvc;
using Monappoly_ASP.Models;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.CardActions.Turn;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Action.Edit;

public class TurnAction(CardService cardService) : ActionPageModel(cardService)
{
    public async Task<IActionResult> OnGet(int cardId, int groupId)
    {
        Types = TurnActionType.Miss.GetSelectList();
        var rtn = await SetupCard(cardId, groupId);
        return rtn ?? Page();
    }
}