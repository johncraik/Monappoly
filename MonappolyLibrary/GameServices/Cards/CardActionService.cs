using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonappolyLibrary.Data;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Cards.CardActions.BoardSpace;
using MonappolyLibrary.GameModels.Cards.CardActions.Dice;
using MonappolyLibrary.GameModels.Cards.CardActions.Money;
using MonappolyLibrary.GameModels.Cards.CardActions.Move;
using MonappolyLibrary.GameModels.Cards.CardActions.Player;
using MonappolyLibrary.GameModels.Cards.CardActions.Property;
using MonappolyLibrary.GameModels.Cards.CardActions.TakeCard;
using MonappolyLibrary.GameModels.Cards.CardActions.Turn;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using Newtonsoft.Json;

namespace MonappolyLibrary.GameServices.Cards;

public class CardActionService
{
    private readonly MonappolyDbContext _context;
    private readonly ILogger<CardActionService> _logger;
    private readonly UserInfo _userInfo;
    private readonly CardActionFileService _cardActionFileService;

    public CardActionService(MonappolyDbContext context,
        ILogger<CardActionService> logger,
        UserInfo userInfo,
        CardActionFileService cardActionFileService)
    {
        _context = context;
        _logger = logger;
        _userInfo = userInfo;
        _cardActionFileService = cardActionFileService;
    }

    public async Task<List<(ActionGroupViewModel Group, List<ActionViewModel>)>> GetGroupViewModels(int cardId)
    {
        var groups = await GetCardActionGroups(cardId);
        return (from g in groups 
            let actions = GetGroupCardActions(g.Id, g.CardId) 
            let actionVms = actions.Select(a => a.ToViewModel()).ToList() 
            select (new ActionGroupViewModel(g), actionVms)).ToList();
    }
    
    public async Task<List<ActionGroup>> GetCardActionGroups(int cardId)
        => await _context.CardActionGroups.Where(cag => cag.CardId == cardId)
            .Include(cag => cag.Card)
            .ThenInclude(c => c.CardType).ToListAsync();

    public async Task<ActionGroup?> FindCardActionGroup(int cardId)
        => await _context.CardActionGroups.Where(cag => cag.CardId == cardId)
            .Include(cag => cag.Card)
            .ThenInclude(c => c.CardType).FirstOrDefaultAsync();
    
    public async Task<ActionGroup?> FindActionGroup(int groupId)
        => await _context.CardActionGroups.Where(cag => cag.Id == groupId)
            .Include(cag => cag.Card)
            .ThenInclude(c => c.CardType).FirstOrDefaultAsync();

    private async Task ValidateActionGroup(ActionGroupViewModel groupViewModel, ModelStateDictionary modelState)
    {
        if (!groupViewModel.IsKeep)
        {
            groupViewModel.PlayCondition = [ActionPlayCondition.Default];
        }
        
        var validCard = await _context.Cards.AnyAsync(c => c.Id == groupViewModel.CardId);
        if (!validCard)
        {
            modelState.AddModelError("Input", "Card not found");
        }
    }
    
    public async Task<bool> AddCardActionGroup(ActionGroupViewModel groupViewModel, ModelStateDictionary modelState)
    {
        await ValidateActionGroup(groupViewModel, modelState);
        if(!modelState.IsValid) return false;
        
        var group = new ActionGroup();
        groupViewModel.Fill(group);
        group.TenantId = _userInfo.TenantId;
        
        group.FillCreated(_userInfo);
        await _context.CardActionGroups.AddAsync(group);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCardActionGroup(ActionGroupViewModel groupViewModel, ModelStateDictionary modelState)
    {
        await ValidateActionGroup(groupViewModel, modelState);
        if(!modelState.IsValid) return false;
        
        var group = await _context.CardActionGroups.FirstOrDefaultAsync(cag => cag.Id == groupViewModel.Id);
        if(group == null) return false;
        
        groupViewModel.Fill(group);
        group.FillModified(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteCardActionGroup(int groupId)
    {
        var group = await _context.CardActionGroups.FirstOrDefaultAsync(cag => cag.Id == groupId);
        if(group == null) return false;
        
        group.FillDeleted(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }

    #region Actions

    private List<ICardAction> GetGroupCardActions(int groupId, int cardId)
    {
        var actionFiles = _cardActionFileService.GetActionFiles(cardId, groupId);
        var actions = new List<ICardAction>();
        foreach (var af in actionFiles)
        {
            if(af.ActionType == null) continue;
            
            var file = _cardActionFileService.GetAction(af.FullPath);
            var type = (ActionType)af.ActionType;
            var action = type switch
            {
                ActionType.Move => GetMoveAction(file),
                ActionType.Dice => GetDiceAction(file),
                ActionType.Turn => GetTurnAction(file),
                ActionType.Money => GetMoneyAction(file),
                ActionType.Player => GetPlayerAction(file),
                ActionType.Property => GetPropertyAction(file),
                ActionType.BoardSpace => GetBoardSpaceAction(file),
                ActionType.TakeCard => GetTakeCardAction(file),
                _ => null
            };
            if(action == null) continue;
            actions.Add(action);
        }

        return actions;
    }

    public async Task<(bool Result, int CardId)> CreateAction(ICardAction action, ModelStateDictionary modelState)
    {
        action.Validate(modelState);
        if(!modelState.IsValid) return (false, 0);
        
        var group = await _context.CardActionGroups.FirstOrDefaultAsync(cag => cag.Id == action.GroupId);
        if (group != null)
        {
            action.Id = group.NextActionId;
            var res = action.Type switch
            {
                ActionType.Move => await SaveMoveAction(action as IMoveAction, group.CardId, group.Id, group.NextActionId),
                ActionType.Dice => await SaveDiceAction(action as IDiceAction, group.CardId, group.Id, group.NextActionId),
                ActionType.Turn => await SaveTurnAction(action as ITurnAction, group.CardId, group.Id, group.NextActionId),
                ActionType.Money => await SaveMoneyAction(action as IMoneyAction, group.CardId, group.Id, group.NextActionId),
                ActionType.Player => await SavePlayerAction(action as IPlayerAction, group.CardId, group.Id, group.NextActionId),
                ActionType.Property => await SavePropertyAction(action as IPropertyAction, group.CardId, group.Id, group.NextActionId),
                ActionType.BoardSpace => await SaveBoardSpaceAction(action as IBoardSpaceAction, group.CardId, group.Id, group.NextActionId),
                ActionType.TakeCard => await SaveTakeCardAction(action as ITakeCardAction, group.CardId, group.Id, group.NextActionId),
                _ => false
            };
            if (res)
            {
                group.NextActionId++;
                group.FillModified(_userInfo);
                await _context.SaveChangesAsync();
                return (true, group.CardId);
            }
        }
        
        modelState.AddModelError("", "Unknown error has occurred. Either the group could not be found or the action could not be saved.");
        return (false, 0);
    }

    #endregion

    
    
    #region Move Actions

    private ICardAction? GetMoveAction(string file)
    {
        var moveAction = JsonConvert.DeserializeObject<BaseMoveAction>(file);
        ICardAction? action = moveAction?.MoveType switch
        {
            MoveActionType.Simple => JsonConvert.DeserializeObject<SimpleMoveAction>(file),
            MoveActionType.Special => JsonConvert.DeserializeObject<SpecialMoveAction>(file),
            MoveActionType.Swap => JsonConvert.DeserializeObject<SwapMoveAction>(file),
            _ => null
        };

        return action;
    }    
    
    private async Task<bool> SaveMoveAction(IMoveAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.MoveType switch
        {
            MoveActionType.Simple => JsonConvert.SerializeObject(action as SimpleMoveAction),
            MoveActionType.Special => JsonConvert.SerializeObject(action as SpecialMoveAction),
            MoveActionType.Swap => JsonConvert.SerializeObject(action as SwapMoveAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.Move, file);
        return true;
    }

    #endregion

    
    
    #region Dice Actions

    private ICardAction? GetDiceAction(string file)
    {
        var diceAction = JsonConvert.DeserializeObject<BaseDiceAction>(file);
        ICardAction? action = diceAction?.DiceType switch
        {
            DiceActionType.Convert => JsonConvert.DeserializeObject<ConvertDiceAction>(file),
            DiceActionType.Downgrade => JsonConvert.DeserializeObject<DowngradeDiceAction>(file),
            DiceActionType.Reroll => JsonConvert.DeserializeObject<RerollDiceAction>(file),
            _ => null
        };

        return action;
    } 
    
    private async Task<bool> SaveDiceAction(IDiceAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.DiceType switch
        {
            DiceActionType.Convert => JsonConvert.SerializeObject(action as ConvertDiceAction),
            DiceActionType.Downgrade => JsonConvert.SerializeObject(action as DowngradeDiceAction),
            DiceActionType.Reroll => JsonConvert.SerializeObject(action as RerollDiceAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.Dice, file);
        return true;
    }

    #endregion

    
    
    #region Turn Actions

    private ICardAction? GetTurnAction(string file)
    {
        var turnAction = JsonConvert.DeserializeObject<BaseTurnAction>(file);
        ICardAction? action = turnAction?.TurnType switch
        {
            TurnActionType.Extra => JsonConvert.DeserializeObject<ExtraTurnAction>(file),
            TurnActionType.Miss => JsonConvert.DeserializeObject<MissTurnAction>(file),
            _ => null
        };
        
        return action;
    }
    
    private async Task<bool> SaveTurnAction(ITurnAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.TurnType switch
        {
            TurnActionType.Extra => JsonConvert.SerializeObject(action as ExtraTurnAction),
            TurnActionType.Miss => JsonConvert.SerializeObject(action as MissTurnAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.Turn, file);
        return true;
    }

    #endregion
    
    
    
    #region Money Actions
    
    private ICardAction? GetMoneyAction(string file)
    {
        var moneyAction = JsonConvert.DeserializeObject<BaseMoneyAction>(file);
        ICardAction? action = moneyAction?.MoneyType switch
        {
            MoneyActionType.Pay => JsonConvert.DeserializeObject<PayMoneyAction>(file),
            MoneyActionType.Receive => JsonConvert.DeserializeObject<ReceiveMoneyAction>(file),
            _ => null
        };

        return action;
    }
    
    private async Task<bool> SaveMoneyAction(IMoneyAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.MoneyType switch
        {
            MoneyActionType.Pay => JsonConvert.SerializeObject(action as PayMoneyAction),
            MoneyActionType.Receive => JsonConvert.SerializeObject(action as ReceiveMoneyAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.Money, file);
        return true;
    }
    
    #endregion
    
    
    
    #region Player Actions
    
    private ICardAction? GetPlayerAction(string file)
    {
        var playerAction = JsonConvert.DeserializeObject<BasePlayerAction>(file);
        ICardAction? action = playerAction?.PlayerType switch
        {
            PlayerActionType.DiceNumber => JsonConvert.DeserializeObject<DiceNumberPlayerAction>(file),
            PlayerActionType.JailCost => JsonConvert.DeserializeObject<JailCostPlayerAction>(file),
            PlayerActionType.TripleBonus => JsonConvert.DeserializeObject<TripleBonusPlayerAction>(file),
            _ => null
        };

        return action;
    }
    
    private async Task<bool> SavePlayerAction(IPlayerAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.PlayerType switch
        {
            PlayerActionType.DiceNumber => JsonConvert.SerializeObject(action as DiceNumberPlayerAction),
            PlayerActionType.JailCost => JsonConvert.SerializeObject(action as JailCostPlayerAction),
            PlayerActionType.TripleBonus => JsonConvert.SerializeObject(action as TripleBonusPlayerAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.Player, file);
        return true;
    }
    
    #endregion
    
    
    
    #region Property Actions
    
    private ICardAction? GetPropertyAction(string file)
    {
        var propertyAction = JsonConvert.DeserializeObject<BasePropertyAction>(file);
        ICardAction? action = propertyAction?.PropertyType switch
        {
            PropertyActionType.Mortgage => JsonConvert.DeserializeObject<MortgagePropertyAction>(file),
            PropertyActionType.Unmortgage => JsonConvert.DeserializeObject<UnmortgagePropertyAction>(file),
            PropertyActionType.Purge => JsonConvert.DeserializeObject<PurgePropertyAction>(file),
            PropertyActionType.Take => JsonConvert.DeserializeObject<TakePropertyAction>(file),
            PropertyActionType.Return => JsonConvert.DeserializeObject<ReturnPropertyAction>(file),
            _ => null
        };

        return action;
    }
    
    private async Task<bool> SavePropertyAction(IPropertyAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.PropertyType switch
        {
            PropertyActionType.Mortgage => JsonConvert.SerializeObject(action as MortgagePropertyAction),
            PropertyActionType.Unmortgage => JsonConvert.SerializeObject(action as UnmortgagePropertyAction),
            PropertyActionType.Purge => JsonConvert.SerializeObject(action as PurgePropertyAction),
            PropertyActionType.Take => JsonConvert.SerializeObject(action as TakePropertyAction),
            PropertyActionType.Return => JsonConvert.SerializeObject(action as ReturnPropertyAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.Property, file);
        return true;
    }
    
    #endregion
    
    
    
    #region BoardSpace Actions
    
    private ICardAction? GetBoardSpaceAction(string file)
    {
        var boardSpaceAction = JsonConvert.DeserializeObject<BaseBoardSpaceAction>(file);
        ICardAction? action = boardSpaceAction?.BoardSpaceType switch
        {
            BoardSpaceActionType.Go => JsonConvert.DeserializeObject<GoBoardSpaceAction>(file),
            BoardSpaceActionType.Jail => JsonConvert.DeserializeObject<JailBoardSpaceAction>(file),
            BoardSpaceActionType.FreeParking => JsonConvert.DeserializeObject<FreeParkingBoardSpaceAction>(file),
            BoardSpaceActionType.Tax => JsonConvert.DeserializeObject<TaxBoardSpaceAction>(file),
            BoardSpaceActionType.Card => JsonConvert.DeserializeObject<CardBoardSpaceAction>(file),
            BoardSpaceActionType.Property => JsonConvert.DeserializeObject<PropertyBoardSpaceAction>(file),
            _ => null
        };

        return action;
    }
    
    private async Task<bool> SaveBoardSpaceAction(IBoardSpaceAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.BoardSpaceType switch
        {
            BoardSpaceActionType.Go => JsonConvert.SerializeObject(action as GoBoardSpaceAction),
            BoardSpaceActionType.Jail => JsonConvert.SerializeObject(action as JailBoardSpaceAction),
            BoardSpaceActionType.FreeParking => JsonConvert.SerializeObject(action as FreeParkingBoardSpaceAction),
            BoardSpaceActionType.Tax => JsonConvert.SerializeObject(action as TaxBoardSpaceAction),
            BoardSpaceActionType.Card => JsonConvert.SerializeObject(action as CardBoardSpaceAction),
            BoardSpaceActionType.Property => JsonConvert.SerializeObject(action as PropertyBoardSpaceAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.BoardSpace, file);
        return true;
    }
    
    #endregion
    
    
    
    #region TakeCard Actions
    
    private ICardAction? GetTakeCardAction(string file)
    {
        var takeCardAction = JsonConvert.DeserializeObject<BaseTakeCardAction>(file);
        ICardAction? action = takeCardAction?.TakeCardType switch
        {
            TakeCardActionType.SingleCard => JsonConvert.DeserializeObject<SingleCardAction>(file),
            _ => null
        };

        return action;
    }
    
    private async Task<bool> SaveTakeCardAction(ITakeCardAction? action, int cardId, int groupId, int actionId)
    {
        if(action == null) return false;
        var file = action.TakeCardType switch
        {
            TakeCardActionType.SingleCard => JsonConvert.SerializeObject(action as SingleCardAction),
            _ => null
        };
        
        if(file == null) return false;
        await _cardActionFileService.SaveAction(cardId, groupId, actionId, (int)ActionType.TakeCard, file);
        return true;
    }
    
    #endregion
}