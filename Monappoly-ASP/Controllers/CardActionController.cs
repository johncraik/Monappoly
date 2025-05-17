using Microsoft.AspNetCore.Mvc;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;
using MonappolyLibrary.GameModels.Cards.CardActions.Dice;
using MonappolyLibrary.GameModels.Cards.CardActions.Money;
using MonappolyLibrary.GameModels.Cards.CardActions.Move;
using MonappolyLibrary.GameModels.Cards.CardActions.Player;
using MonappolyLibrary.GameModels.Cards.CardActions.Property;
using MonappolyLibrary.GameModels.Cards.CardActions.Turn;
using MonappolyLibrary.GameServices.Cards;
using Index = Monappoly_ASP.Pages.Objects.Cards.Action.Index;

namespace Monappoly_ASP.Controllers;

public class CardActionController : Controller
{
    private readonly CardActionService _cardActionService;

    public CardActionController(CardActionService cardActionService)
    {
        _cardActionService = cardActionService;
    }

    private async Task<IActionResult> CreateAction(ICardAction model)
    {
        var (res, cardId) = await _cardActionService.CreateAction(model, ModelState);
        return res
            ? RedirectToPage($"/Objects/Cards/Action/{nameof(Index)}", new { cardId, groupId = model.GroupId })
            : BadRequest(ModelState);
    }
    
    #region Move Actions

    public IActionResult GetMoveActionPartial(MoveActionType type)
        => PartialView("Objects/Cards/MoveAction/_BaseMoveActionPartial", type);
    
    [HttpPost]
    public async Task<IActionResult> SimpleMoveAction(SimpleMoveAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> SpecialMoveAction(SpecialMoveAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> SwapMoveAction(SwapMoveAction model)
        => await CreateAction(model);

    #endregion

    
    
    #region Dice Actions

    public IActionResult GetDiceActionPartial(DiceActionType type)
        => PartialView("Objects/Cards/DiceAction/_BaseDiceActionPartial", type);
    
    [HttpPost]
    public async Task<IActionResult> ConvertDiceAction(ConvertDiceAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> DowngradeDiceAction(DowngradeDiceAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> RerollDiceAction(RerollDiceAction model)
        => await CreateAction(model);

    #endregion
    
    
    
    #region Turn Actions

    public IActionResult GetTurnActionPartial(TurnActionType type)
        => PartialView("Objects/Cards/TurnAction/_BaseTurnActionPartial", type);
    
    [HttpPost]
    public async Task<IActionResult> ExtraTurnAction(ExtraTurnAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> MissTurnAction(MissTurnAction model)
        => await CreateAction(model);

    #endregion
    
    
    
    #region Money Actions

    public IActionResult GetMoneyActionPartial(MoneyActionType type)
        => PartialView("Objects/Cards/MoneyAction/_BaseMoneyActionPartial", type);
    
    [HttpPost]
    public async Task<IActionResult> PayMoneyAction(PayMoneyAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> ReceiveMoneyAction(ReceiveMoneyAction model)
        => await CreateAction(model);

    #endregion
    
    
    
    #region Player Actions

    public IActionResult GetPlayerActionPartial(PlayerActionType type)
        => PartialView("Objects/Cards/PlayerAction/_BasePlayerActionPartial", type);
    
    [HttpPost]
    public async Task<IActionResult> DiceNumberPlayerAction(DiceNumberPlayerAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> JailCostPlayerAction(JailCostPlayerAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> TripleBonusPlayerAction(TripleBonusPlayerAction model)
        => await CreateAction(model);

    #endregion
    
    
    
    #region Dice Actions

    public IActionResult GetPropertyActionPartial(PropertyActionType type)
        => PartialView("Objects/Cards/PropertyAction/_BasePropertyActionPartial", type);
    
    [HttpPost]
    public async Task<IActionResult> MortgagePropertyAction(MortgagePropertyAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> UnmortgagePropertyAction(UnmortgagePropertyAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> PurgePropertyAction(PurgePropertyAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> TakePropertyAction(TakePropertyAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> ReturnPropertyAction(ReturnPropertyAction model)
        => await CreateAction(model);

    #endregion
    
    
    
    #region Dice Actions

    public IActionResult GetBoardSpaceActionPartial(BoardSpaceActionType type)
        => PartialView("Objects/Cards/BoardSpaceAction/_BaseBoardSpaceActionPartial", type);
    
    [HttpPost]
    public async Task<IActionResult> GoBoardSpaceAction(GoBoardSpaceAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> JailBoardSpaceAction(JailBoardSpaceAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> FreeParkingBoardSpaceAction(FreeParkingBoardSpaceAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> TaxBoardSpaceAction(TaxBoardSpaceAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> CardBoardSpaceAction(CardBoardSpaceAction model)
        => await CreateAction(model);
    
    [HttpPost]
    public async Task<IActionResult> PropertyBoardSpaceAction(PropertyBoardSpaceAction model)
        => await CreateAction(model);

    #endregion
}