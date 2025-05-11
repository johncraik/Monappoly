using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Primitives;

namespace MonappolyLibrary.Configuration;

public class Configuration : IConfiguration
{
    private readonly IConfigurationRoot _innerConfig;

    public Configuration()
    {
        _innerConfig = new ConfigurationBuilder()
            .AddJsonFile($"{Environment.CurrentDirectory}/../MonappolyLibrary/Configuration/Configuration.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public IConfigurationSection GetSection(string key) => _innerConfig.GetSection(key);

    public IEnumerable<IConfigurationSection> GetChildren() => _innerConfig.GetChildren();

    public IChangeToken GetReloadToken() => _innerConfig.GetReloadToken();

    public string? this[string key]
    {
        get => _innerConfig[key];
        set => _innerConfig[key] = value;
    }
}