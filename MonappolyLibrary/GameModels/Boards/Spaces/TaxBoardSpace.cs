using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Boards.Spaces;

public class TaxBoardSpace : DataModel, IBoardSpace
{
    public int Id { get; set; }
    
    public uint BoardIndex { get; set; }
    public string Name { get; set; }
    public BoardSpaceType SpaceType { get; set; } = BoardSpaceType.Tax;
    
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public virtual Board Board { get; set; }
    
    public uint TaxAmount { get; set; }
    
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
    }
}