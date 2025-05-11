namespace Monappoly_ASP;

public class Version
{
    private readonly IConfiguration _config;

    public Version(IConfiguration config)
    {
        _config = config;
    }
    
    public string GetVersion()
    {
        var v = _config["version"];
        return v == null ? "No version found" : $"v{v}";
    }

    public void UpdateVersion(string v)
    {
        _config["version"] = v;
    }
}