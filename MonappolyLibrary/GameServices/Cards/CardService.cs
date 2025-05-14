using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameModels.Cards.ViewModels;
using SQLitePCL;

namespace MonappolyLibrary.GameServices.Cards;

public class CardService
{
    private readonly MonappolyDbContext _context;
    private readonly UserInfo _userInfo;

    public CardService(MonappolyDbContext context, UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }


    #region Cards

    //Queries
    public async Task<List<CardViewModel>> GetCards(int deckId)
    {
        var cards = await _context.Cards
            .Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => c.CardDeckId == deckId)
            .OrderBy(c => c.CardType.TenantId).ThenBy(c => c.CardType.Name)
            .ToListAsync();

        var vms = new List<CardViewModel>();
        foreach (var card in cards)
        {
            var hasAction = await _context.CardActionGroups.AnyAsync(ac => ac.CardId == card.Id);
            var vm = new CardViewModel(card, hasAction);
            vms.Add(vm);
        }

        return vms;
    }

    public async Task<List<Card>> GetBaseCards(int deckId)
        => await _context.Cards
            .Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .Where(c => c.CardDeckId == deckId)
            .OrderBy(c => c.CardType.TenantId).ThenBy(c => c.CardType.Name)
            .ToListAsync();
    
    public async Task<Card?> FindCard(int id)
        => await _context.Cards
            .Include(c => c.CardType)
            .Include(c => c.CardDeck)
            .FirstOrDefaultAsync(c => c.Id == id);
    
    
    //Adding, modifying, deleting:
    private async Task<bool> ValidateCard(Card card, ModelStateDictionary modelState)
    {
        if (!card.IsModifiable())
        {
            modelState.AddModelError("Input.Text", "Card cannot be modified.");
            return false;
        }

        var deck = await FindDeck(card.CardDeckId);
        if(deck == null)
        {
            modelState.AddModelError("Input.Deck", "Deck not found.");
        }
        else if (!deck.IsModifiable())
        {
            modelState.AddModelError("Input.Deck", "Deck cannot be modified.");
            return false;
        }
        
        var type = await FindType(card.CardTypeId);
        if(type == null)
        {
            modelState.AddModelError("Input.Type", "Type not found.");
        }
        
        var existingCard = await _context.Cards.AnyAsync(c => c.CardDeckId == card.CardDeckId
            && c.CardTypeId == card.CardTypeId
            && c.Text == card.Text
            && c.Id != card.Id);
        if (existingCard)
        {
            modelState.AddModelError("Input.Text", "Card already exists.");
        }

        return true;
    }
    
    public async Task<bool> TryAddCard(Card card, ModelStateDictionary modelState)
    {
        var res = await ValidateCard(card, modelState);
        if(!res || !modelState.IsValid) return false;
        
        card.FillCreated(_userInfo);
        await _context.Cards.AddAsync(card);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdateCard(Card card, ModelStateDictionary modelState)
    {
        var res = await ValidateCard(card, modelState);
        if(!res || !modelState.IsValid) return false;
        
        card.FillModified(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    public async Task<bool> TryDeleteCard(int id)
    {
        var card = await FindCard(id);
        if(card == null) return false;
        if(!card.IsDeletable()) return false;
        
        card.FillDeleted(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task DeleteAllCards(int deckId)
    {
        var cards = await _context.Cards
            .Where(c => c.CardDeckId == deckId)
            .ToListAsync();
        
        foreach (var card in cards)
        {
            card.FillDeleted(_userInfo);
        }
        
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Card Decks

    //Queries
    public async Task<List<CardDeck>> GetDecks() => await _context.CardDecks
        .OrderByDescending(d => d.TenantId).ThenBy(d => d.Name)
        .ToListAsync();
    
    public async Task<CardDeck?> FindDeck(int id)
        => await _context.CardDecks.FirstOrDefaultAsync(d => d.Id == id);
    
    
    //Adding, modifying, deleting:
    private async Task<bool> ValidateDeck(CardDeck deck, ModelStateDictionary modelState)
    {
        if (!deck.IsModifiable())
        {
            modelState.AddModelError("Input.Name", "Deck cannot be modified.");
            return false;
        }
        
        var existingDeck = await _context.CardDecks.AnyAsync(d => d.Name == deck.Name 
                                                                  && d.Id != deck.Id);
        if (existingDeck)
        {
            modelState.AddModelError("Input.Name", "Deck already exists.");
        }
        return true;
    }
    
    public async Task<bool> TryAddDeck(CardDeck deck, ModelStateDictionary modelState)
    {
        var res = await ValidateDeck(deck, modelState);
        if(!res || !modelState.IsValid) return false;
        
        deck.FillCreated(_userInfo);
        await _context.CardDecks.AddAsync(deck);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdateDeck(CardDeck deck, ModelStateDictionary modelState)
    {
        var res = await ValidateDeck(deck, modelState);
        if(!res || !modelState.IsValid) return false;
        
        deck.FillModified(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TryDeleteDeck(int id)
    {
        var deck = await FindDeck(id);
        if(deck == null) return false;
        if (!deck.IsDeletable()) return false;

        deck.FillDeleted(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }
    

    #endregion


    #region Card Types
    
    public async Task<List<CardType>> GetTypes() => await _context.CardTypes
        .OrderByDescending(t => t.TenantId).ThenBy(t => t.Name)
        .ToListAsync();
    
    public async Task<CardType?> FindType(int id)
        => await _context.CardTypes.FirstOrDefaultAsync(t => t.Id == id);

    private async Task<bool> ValidateType(CardType type, ModelStateDictionary modelState)
    {
        if (!type.IsModifiable())
        {
            modelState.AddModelError("Input.Name", "Type cannot be modified.");
            return false;
        }
        
        var existingType = await _context.CardTypes.AnyAsync(t => t.Name == type.Name 
                                                                  && t.Id != type.Id);
        if (existingType)
        {
            modelState.AddModelError("Input.Name", "Type already exists.");
        }
        var existingColour = await _context.CardTypes.AnyAsync(t => t.Colour == type.Colour 
                                                                  && t.Id != type.Id);
        if (existingColour)
        {
            modelState.AddModelError("Input.Colour", "This colour already exists.");
        }
        
        return true;
    }
    
    public async Task<bool> TryAddType(CardType type, ModelStateDictionary modelState)
    {
        var res = await ValidateType(type, modelState);
        if(!res || !modelState.IsValid) return false;
        
        type.FillCreated(_userInfo);
        await _context.CardTypes.AddAsync(type);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdateType(CardType type, ModelStateDictionary modelState)
    {
        var res = await ValidateType(type, modelState);
        if(!res || !modelState.IsValid) return false;
        
        type.FillModified(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryDeleteType(int id)
    {
        var type = await FindType(id);
        if(type == null) return false;
        if (!type.IsDeletable()) return false;

        type.FillDeleted(_userInfo);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion
        
}