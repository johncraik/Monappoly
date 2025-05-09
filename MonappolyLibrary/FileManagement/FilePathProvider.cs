namespace MonappolyLibrary.FileManagement;

public class FilePathProvider
{
    private readonly string _basePath;

    public FilePathProvider()
    {
        var config = new Configuration.Configuration();
        _basePath = config["FILE_BASE_PATH"]!;

        if (string.IsNullOrEmpty(_basePath))
        {
            throw new Exception("Base path not set in configuration.");
        }
    }

    public string GetFilePath(FileType type, int tenantId)
    {
        var basePath = Path.Combine(_basePath, "tenant-files");

        var path = type switch
        {
            FileType.CardAction => Path.Combine(basePath, "CardActions"),
            FileType.Boards => Path.Combine(basePath, "Boards"),
            FileType.Cards => Path.Combine(basePath, "Cards"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        return path;
    }

}

public enum FileType
{
    CardAction,
    Boards,
    Cards
}