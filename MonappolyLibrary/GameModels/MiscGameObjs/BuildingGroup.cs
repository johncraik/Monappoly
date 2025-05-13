using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.MiscGameObjs;

public class BuildingGroup : DataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}