using MonappolyLibrary.Data;
using MonappolyLibrary.Services.Defaults;

namespace MonappolyLibrary.Services;

public class DefaultsService
{
    private readonly MonappolyDbContext _context;
    private readonly CardDefaultsService _cardDefaultsService;

    public DefaultsService(MonappolyDbContext context)
    {
        _context = context;
        _cardDefaultsService = new CardDefaultsService(context, new CsvReader<CardDefaultsService.CardUpload>());
    }

    public async Task EnsureDefaults()
    {
        await _cardDefaultsService.EnsureCardDefaults();
    }
}