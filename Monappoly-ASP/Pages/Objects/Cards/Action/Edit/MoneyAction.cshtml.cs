using Microsoft.AspNetCore.Mvc;
using Monappoly_ASP.Models;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.CardActions.Money;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Action.Edit;

public class MoneyAction(CardService cardService) : ActionPageModel(cardService)
{
    public async Task<IActionResult> OnGet(int cardId, int groupId)
    {
        Types = MoneyActionType.Pay.GetSelectList();
        var rtn = await SetupCard(cardId, groupId);
        return rtn ?? Page();
    }
}