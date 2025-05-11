using MonappolyLibrary.Data;

namespace MonappolyLibrary.GameServices.Cards;

public class CardService
{
    private readonly MonappolyDbContext _context;

    public CardService(MonappolyDbContext context)
    {
        _context = context;
    }
    
}