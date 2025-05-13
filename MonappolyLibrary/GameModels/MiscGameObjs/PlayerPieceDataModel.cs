using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.MiscGameObjs;

public class PlayerPieceDataModel : DataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
}