using MonappolyLibrary.FileManagement;
using MonappolyLibrary.GameModels.Cards.CardActions;

namespace MonappolyLibrary.GameServices.Cards;

public class CardActionFileService
{
    private readonly FilePathProvider _filePathProvider;
    private readonly UserInfo _userInfo;

    public CardActionFileService(FilePathProvider filePathProvider,
        UserInfo userInfo)
    {
        _filePathProvider = filePathProvider;
        _userInfo = userInfo;
    }

    private string GetCardPath(int cardId)
    {
        var path = Path.Combine(_filePathProvider.GetFilePath(FileType.CardAction, _userInfo.TenantId), cardId.ToString());
        EnsureDirectoryExists(path);
        return path;
    }
    
    private string GetGroupPath(int cardId, int groupId)
    {
        var path = Path.Combine(GetCardPath(cardId), groupId.ToString());
        EnsureDirectoryExists(path);
        return path;
    }
    
    // private void EnsureDirectoryExists(int cardId, int actionId = -1)
    // {
    //     var path = GetCardPath(cardId);
    //     if (!Directory.Exists(path))
    //     {
    //         Directory.CreateDirectory(path);
    //     }
    //     
    //     if(actionId == -1) return;
    //     path = GetGroupPath(cardId, actionId);
    //     if (!Directory.Exists(path))
    //     {
    //         Directory.CreateDirectory(path);
    //     }
    // }
    
    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    
    public bool GroupHasActions(int cardId, int groupId)
    {
        var path = GetGroupPath(cardId, groupId);
        
        var files = Directory.GetFiles(path);
        return files.Length > 0;
    }

    public List<(string FullPath, string FileName, int? ActionType)> GetActionFiles(int cardId, int groupId)
    {
        var path = GetGroupPath(cardId, groupId);
        
        var files = Directory.GetFiles(path);
        if(files.Length == 0) return new List<(string, string, int?)>();
        
        var actionFiles = new List<(string FullPath, string FileName, int? ActionType)>();
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            
            int.TryParse(fileName.Split('_', StringSplitOptions.RemoveEmptyEntries)[0], out var typeParsed);
            actionFiles.Add((file, fileName, typeParsed));
        }

        return actionFiles;
    }

    public string GetAction(string path) => _filePathProvider.GetFile(path);


    public async Task SaveAction(int cardId, int groupId, int actionId, int actionType, string serialisedAction)
    {
        var path = GetGroupPath(cardId, groupId);
        path = Path.Combine(path, $"{actionType}_{actionId}.txt");
        
        await using var ws = new StreamWriter(path);
        await ws.WriteLineAsync(serialisedAction);
        await ws.FlushAsync();
    }
}