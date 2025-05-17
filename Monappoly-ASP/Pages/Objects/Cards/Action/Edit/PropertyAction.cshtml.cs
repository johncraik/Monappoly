using Microsoft.AspNetCore.Mvc;
using Monappoly_ASP.Models;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.GameModels.Cards.CardActions.Property;
using MonappolyLibrary.GameServices.Cards;

namespace Monappoly_ASP.Pages.Objects.Cards.Action.Edit;

public class PropertyAction(CardService cardService) : ActionPageModel(cardService)
{
    public async Task<IActionResult> OnGet(int cardId, int groupId)
    {
        Types = PropertyActionType.Take.GetSelectList();
        var rtn = await SetupCard(cardId, groupId);
        return rtn ?? Page();
    }
}