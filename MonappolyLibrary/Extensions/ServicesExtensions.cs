using Microsoft.Extensions.DependencyInjection;
using MonappolyLibrary.FileManagement;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.GameServices.Cards;
using MonappolyLibrary.Services;
using MonappolyLibrary.Services.Defaults;

namespace MonappolyLibrary.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection GetGameService(this IServiceCollection services)
    {
        services.AddTransient<CardService>();
        services.AddTransient<CardActionService>();

        return services;
    }
    
    public static IServiceCollection GetServices(this IServiceCollection services)
    {
        services.AddTransient<FilePathProvider>();
        services.AddTransient<CsvReader<CardDefaultsService.CardUpload>>();

        return services;
    }
}