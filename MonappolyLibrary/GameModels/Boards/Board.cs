using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Boards;

public class Board : DataModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}