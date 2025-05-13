using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Boards.Spaces;

public class PropertyBoardSpace : DataModel, IBoardSpace
{
    public int Id { get; set; }
    public uint BoardIndex { get; set; }
    public string Name { get; set; }
    public BoardSpaceType SpaceType { get; set; } = BoardSpaceType.Property;
    
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public virtual Board Board { get; set; }
    
    public uint Cost { get; set; }
    public PropertyType PropertyType { get; set; }
    public PropertySet PropertySet { get; set; }
    
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (SpaceType != BoardSpaceType.Tax)
        {
            throw new InvalidOperationException("SpaceType must be Tax for TaxBoardSpace.");
        }

        if (string.IsNullOrEmpty(Name))
        {
            modelState.AddModelError("Name", "Name cannot be empty.");
        }
        
        if(BoardId <= 0)
        {
            modelState.AddModelError("BoardId", "You must select a board.");
        }

        if (PropertyType != PropertyType.SetProperty && PropertySet == PropertySet.None)
        {
            modelState.AddModelError("PropertySet", "You must select a property set.");
        }
    }
}

public enum PropertyType
{
    SetProperty,
    Station,
    Utility
}

public enum PropertySet
{
    None = -1,
    Brown,
    LightBlue,
    Pink,
    Orange,
    Red,
    Yellow,
    Green,
    DarkBlue
}

public class PropertySpaceUpload : BoardSpaceUpload
{
    public uint Cost { get; set; }
    public PropertyType PropertyType { get; set; }
    public PropertySet PropertySet { get; set; }
}