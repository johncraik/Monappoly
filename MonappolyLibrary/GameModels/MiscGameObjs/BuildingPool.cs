using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.MiscGameObjs;

public class BuildingPool : DataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public uint Count { get; set; }
    
    public int BuildingGroupId { get; set; }
    [ForeignKey(nameof(BuildingGroupId))]
    public virtual BuildingGroup BuildingGroup { get; set; }
}