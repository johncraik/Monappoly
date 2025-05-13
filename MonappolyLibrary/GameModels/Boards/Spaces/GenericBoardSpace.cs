using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Boards.Spaces;

public class GenericBoardSpace : DataModel, IBoardSpace
{
    public int Id { get; set; }
    public uint BoardIndex { get; set; }
    public string Name { get; set; }
    public BoardSpaceType SpaceType { get; set; } = BoardSpaceType.Generic;
    
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public virtual Board Board { get; set; }

    public GenericSpaceAction Action { get; set; } 
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (SpaceType != BoardSpaceType.Generic)
        {
            throw new InvalidOperationException("SpaceType must be Generic for GenericBoardSpace.");
        }

        if (string.IsNullOrEmpty(Name))
        {
            modelState.AddModelError("Name", "Name cannot be empty.");
        }
        
        if(BoardId <= 0)
        {
            modelState.AddModelError("BoardId", "You must select a board.");
        }
    }
}

public enum GenericSpaceAction
{
    Go,
    Jail,
    FreeParking,
    GoToJail
}

public class GenericSpaceUpload : BoardSpaceUpload
{
    public GenericSpaceAction Action { get; set; }
}