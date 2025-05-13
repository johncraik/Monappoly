using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MonappolyLibrary.GameModels.Boards.Spaces;

public interface IBoardSpace
{
    int Id { get; set; }
    uint BoardIndex { get; set; }
    string Name { get; set; }
    BoardSpaceType SpaceType { get; set; }
    int BoardId { get; set; }
    
    void Validate(ModelStateDictionary modelState);
}

public enum BoardSpaceType
{
    Generic,
    Tax,
    Card,
    Property
}

public class BoardSpaceUpload
{
    public int Index { get; set; }
    public string Name { get; set; }
    public BoardSpaceType Type { get; set; }
}