using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonappolyLibrary.Data;
using MonappolyLibrary.Extensions;
using MonappolyLibrary.FileManagement;
using MonappolyLibrary.GameModels.Cards.CardActions;
using MonappolyLibrary.GameModels.Cards.CardActions.Dice;
using MonappolyLibrary.GameModels.Cards.CardActions.Move;
using MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;
using Newtonsoft.Json;

namespace MonappolyLibrary.GameServices.Cards;

public class CardActionService
{
    private readonly MonappolyDbContext _context;
    private readonly ILogger<CardActionService> _logger;
    private readonly UserInfo _userInfo;
    private readonly FilePathProvider _filePathProvider;

    public CardActionService(MonappolyDbContext context,
        ILogger<CardActionService> logger,
        UserInfo userInfo,
        FilePathProvider filePathProvider)
    {
        _context = context;
        _logger = logger;
        _userInfo = userInfo;
        _filePathProvider = filePathProvider;
    }

    public async Task<List<ActionGroup>> GetCardActionGroups(int cardId)
        => await _context.CardActionGroups.Where(cag => cag.CardId == cardId)
            /*.Include(cag => cag.Card)
            .ThenInclude(c => c.CardType)*/.ToListAsync();

    public async Task<ActionGroup?> FindCardActionGroup(int cardId)
        => await _context.CardActionGroups.Where(cag => cag.CardId == cardId)
            .Include(cag => cag.Card)
            .ThenInclude(c => c.CardType).FirstOrDefaultAsync();

    public async Task AddCardActionGroup(ActionGroupViewModel groupViewModel)
    {
        var group = new ActionGroup();
        groupViewModel.Fill(group);
        group.FillCreated(_userInfo);
        await _context.CardActionGroups.AddAsync(group);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateCardActionGroup(ActionGroupViewModel groupViewModel)
    {
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

    public async Task<List<ICardAction>> GetAllCardActions(int cardId)
    { 
        var groups = await GetCardActionGroups(cardId);
        var actions = new List<ICardAction>();
        foreach (var group in groups)
        {
            var basePath = _filePathProvider.GetFilePath(FileType.CardAction, _userInfo.TenantId);
            var path = Path.Combine(basePath, group.CardId.ToString(), group.Id.ToString());
            
            var actionFiles = Directory.GetFiles(path);
            foreach (var af in actionFiles)
            {
                if (!int.TryParse(af[(af.LastIndexOf('\\')+1)..].Split('_', StringSplitOptions.RemoveEmptyEntries)[0], out var typeParsed)) 
                    continue;

                var file = _filePathProvider.GetFile(af);
                var type = (ActionType)typeParsed;
                var action = type switch
                {
                    ActionType.Move => GetMoveAction(file),
                    _ => null
                };
            }
        }

        return actions;
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

    #endregion
}