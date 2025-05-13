using Microsoft.Extensions.DependencyInjection;
using MonappolyLibrary.FileManagement;
using MonappolyLibrary.GameServices.Cards;

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

        return services;
    }
}