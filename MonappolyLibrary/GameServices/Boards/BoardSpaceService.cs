using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;
using MonappolyLibrary.GameModels.Boards.Spaces;

namespace MonappolyLibrary.GameServices.Boards;

public class BoardSpaceService
{
    private readonly MonappolyDbContext _context;
    private readonly UserInfo _userInfo;

    public BoardSpaceService(MonappolyDbContext context,
        UserInfo userInfo)
    {
        _context = context;
        _userInfo = userInfo;
    }

    public async Task<bool> EmptySpace(int boardId, int index)
    {
        var notEmpty = await _context.GenericBoardSpaces
            .AnyAsync(s => s.BoardId == boardId && s.BoardIndex == index);
        if (notEmpty) return false;
        
        notEmpty = await _context.TaxBoardSpaces
            .AnyAsync(s => s.BoardId == boardId && s.BoardIndex == index);
        if (notEmpty) return false;
        
        notEmpty = await _context.CardBoardSpaces
            .AnyAsync(s => s.BoardId == boardId && s.BoardIndex == index);
        if (notEmpty) return false;
        
        notEmpty = await _context.PropertyBoardSpaces
            .AnyAsync(s => s.BoardId == boardId && s.BoardIndex == index);
        return !notEmpty;
    }
    
    public async Task<bool> TryReplaceSpace(IBoardSpace currentSpace, IBoardSpace space,
        ModelStateDictionary modelState)
    {
        if (currentSpace.GetType() == space.GetType())
        {
            modelState.AddModelError("Input", "You cannot replace a space with the same type.");
            return false;
        }
        
        //Delete the current space:
        var res = DeleteSpace(currentSpace);
        if(!res) return false;
        
        //Add the new space:
        if(space.GetType() == typeof(GenericBoardSpace))
        {
            if (space is not GenericBoardSpace gs) return false;
            await ValidateGenericSpace(gs, modelState);
            if (!modelState.IsValid) return false;
            gs.FillCreated();
            _context.GenericBoardSpaces.Add(gs);
        }
        else if (space.GetType() == typeof(TaxBoardSpace))
        {
            if(space is not TaxBoardSpace ts) return false;
            await ValidateTaxSpace(ts, modelState);
            if (!modelState.IsValid) return false;
            ts.FillCreated();
            _context.TaxBoardSpaces.Add(ts);
        }
        else if (space.GetType() == typeof(CardBoardSpace))
        {
            if(space is not CardBoardSpace cs) return false;
            await ValidateCardSpace(cs, modelState);
            if (!modelState.IsValid) return false;
            cs.FillCreated();
            _context.CardBoardSpaces.Add(cs);
        }
        else if (space.GetType() == typeof(PropertyBoardSpace))
        {
            if(space is not PropertyBoardSpace ps) return false;
            await ValidatePropertySpace(ps, modelState);
            if (!modelState.IsValid) return false;
            ps.FillCreated();
            _context.PropertyBoardSpaces.Add(ps);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public bool DeleteSpace(IBoardSpace space)
    {
        if(space.GetType() == typeof(GenericBoardSpace))
        {
            if (space is not GenericBoardSpace gs) return false;
            gs.FillDeleted(_userInfo);
        }
        else if (space.GetType() == typeof(TaxBoardSpace))
        {
            if(space is not TaxBoardSpace ts) return false;
            ts.FillDeleted(_userInfo);
        }
        else if (space.GetType() == typeof(CardBoardSpace))
        {
            if(space is not CardBoardSpace cs) return false;
            cs.FillDeleted(_userInfo);
        }
        else if (space.GetType() == typeof(PropertyBoardSpace))
        {
            if(space is not PropertyBoardSpace ps) return false;
            ps.FillDeleted(_userInfo);   
        }

        return true;
    }

    private async Task<bool> ValidateSpaceName(IBoardSpace space)
    {
        var exists = await _context.GenericBoardSpaces
            .AnyAsync(s => s.BoardId == space.BoardId && s.Name == space.Name && s.Id != space.Id);
        if (exists) return false;
        
        exists = await _context.TaxBoardSpaces
            .AnyAsync(s => s.BoardId == space.BoardId && s.Name == space.Name && s.Id != space.Id);
        if (exists) return false;
        
        exists = await _context.CardBoardSpaces
            .AnyAsync(s => s.BoardId == space.BoardId && s.Name == space.Name && s.Id != space.Id);
        if (exists) return false;
        
        exists = await _context.PropertyBoardSpaces
            .AnyAsync(s => s.BoardId == space.BoardId && s.Name == space.Name && s.Id != space.Id);
        return !exists;
    }


    public async Task<IBoardSpace?> FindSpace(int boardId, int index)
    {
        var genericSpace = await _context.GenericBoardSpaces
            .FirstOrDefaultAsync(s => s.BoardId == boardId && s.BoardIndex == index);
        if (genericSpace != null) return genericSpace;
        
        var taxSpace = await _context.TaxBoardSpaces
            .FirstOrDefaultAsync(s => s.BoardId == boardId && s.BoardIndex == index);
        if (taxSpace != null) return taxSpace;
        
        var cardSpace = await _context.CardBoardSpaces
            .FirstOrDefaultAsync(s => s.BoardId == boardId && s.BoardIndex == index);
        if (cardSpace != null) return cardSpace;
        
        return await _context.PropertyBoardSpaces
            .FirstOrDefaultAsync(s => s.BoardId == boardId && s.BoardIndex == index);
    }
    
    
    #region Generic Spaces

    public async Task<GenericBoardSpace?> FindGenericSpace(int spaceId) =>
        await _context.GenericBoardSpaces
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == spaceId);
    
    private async Task ValidateGenericSpace(GenericBoardSpace space, ModelStateDictionary modelState)
    {
        var exists = await ValidateSpaceName(space);
        if (!exists)
        {
            modelState.AddModelError($"Input.{nameof(space.Name)}", "Space with this name already exists.");
        }
        
        if (space.Action == GenericSpaceAction.Jail)
        {
            var hasJail = await _context.GenericBoardSpaces
                .AnyAsync(s => s.BoardId == space.BoardId && s.Action == GenericSpaceAction.Jail && s.Id != space.Id);
            if (hasJail)
            {
                modelState.AddModelError($"Input.{nameof(space.Action)}", "Board already has a Jail space.");
            }
        }
        
        space.Validate(modelState);
    }
    
    public async Task<bool> TryAddGenericSpace(GenericBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidateGenericSpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillCreated();
        _context.GenericBoardSpaces.Add(space);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdateGenericSpace(GenericBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidateGenericSpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillModified();
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion
    
    
    
    #region Tax Spaces
    
    public async Task<TaxBoardSpace?> FindTaxSpace(int spaceId) =>
        await _context.TaxBoardSpaces
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == spaceId);

    private async Task ValidateTaxSpace(TaxBoardSpace space, ModelStateDictionary modelState)
    {
        var exists = await ValidateSpaceName(space);
        if (!exists)
        {
            modelState.AddModelError($"Input.{nameof(space.Name)}", "Space with this name already exists.");
        }
        
        space.Validate(modelState);
    }
    
    public async Task<bool> TryAddTaxSpace(TaxBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidateTaxSpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillCreated();
        _context.TaxBoardSpaces.Add(space);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdateTaxSpace(TaxBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidateTaxSpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillModified();
        await _context.SaveChangesAsync();
        return true;
    }
    
    #endregion
    
    
    
    #region Card Spaces
    
    public async Task<CardBoardSpace?> FindCardSpace(int spaceId) =>
        await _context.CardBoardSpaces
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == spaceId);

    private async Task ValidateCardSpace(CardBoardSpace space, ModelStateDictionary modelState)
    {
        var validCardType = await _context.CardTypes.AnyAsync(t => t.Id == space.CardTypeId);
        if(!validCardType)
        {
            modelState.AddModelError($"Input.{nameof(space.CardTypeId)}", "Card type does not exist.");
        }
        
        space.Validate(modelState);
    }
    
    public async Task<bool> TryAddCardSpace(CardBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidateCardSpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillCreated();
        _context.CardBoardSpaces.Add(space);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdateCardSpace(CardBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidateCardSpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillModified();
        await _context.SaveChangesAsync();
        return true;
    }
    
    #endregion
    
    
    
    #region Property Spaces
    
    public async Task<PropertyBoardSpace?> FindPropertySpace(int spaceId) =>
        await _context.PropertyBoardSpaces
            .Include(s => s.Board)
            .FirstOrDefaultAsync(s => s.Id == spaceId);

    private async Task ValidatePropertySpace(PropertyBoardSpace space, ModelStateDictionary modelState)
    {
        var exists = await ValidateSpaceName(space);
        if (!exists)
        {
            modelState.AddModelError($"Input.{nameof(space.Name)}", "Space with this name already exists.");
        }
        
        var validPropertyType = Enum.IsDefined(typeof(PropertyType), space.PropertyType);
        if (!validPropertyType)
        {
            modelState.AddModelError($"Input.{nameof(space.PropertyType)}", "Invalid property type.");
        }

        if (space.PropertyType == PropertyType.SetProperty)
        {
            var validPropertySet = Enum.IsDefined(typeof(PropertySet), space.PropertySet);
            if (!validPropertySet)
            {
                modelState.AddModelError($"Input.{nameof(space.PropertySet)}", "Invalid property set.");
            }
        }
        
        
        space.Validate(modelState);
    }
    
    public async Task<bool> TryAddPropertySpace(PropertyBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidatePropertySpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillCreated();
        _context.PropertyBoardSpaces.Add(space);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> TryUpdatePropertySpace(PropertyBoardSpace space, ModelStateDictionary modelState)
    {
        await ValidatePropertySpace(space, modelState);
        if (!modelState.IsValid) return false;

        space.FillModified();
        await _context.SaveChangesAsync();
        return true;
    }
    
    #endregion
}