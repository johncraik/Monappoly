using MonappolyLibrary.Data;
using MonappolyLibrary.Services.Defaults;

namespace MonappolyLibrary.Services;

public class DefaultsService
{
    private readonly CardDefaultsService _cardDefaultsService;
    private readonly BoardDefaultsService _boardDefaultsService;
    public static string DefaultsPath { get; private set; } = "";

    public DefaultsService(MonappolyDbContext context)
    {
        _cardDefaultsService = new CardDefaultsService(context, new CsvReader<CardDefaultsService.CardUpload>());
        _boardDefaultsService = new BoardDefaultsService(context);
        DefaultsPath = $"{Environment.CurrentDirectory}/../MonappolyLibrary/Data/Defaults/";
    }

    public async Task EnsureDefaults()
    {
        await _cardDefaultsService.EnsureCardDefaults();
        await _boardDefaultsService.EnsureBoardDefaults();
    }
}