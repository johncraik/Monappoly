using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.GameModels.MiscGameObjs;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Boards;

public class Board : DataModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    public int BuildingGroupId { get; set; }
    [ForeignKey(nameof(BuildingGroupId))]
    public virtual BuildingGroup BuildingGroup { get; set; }
}