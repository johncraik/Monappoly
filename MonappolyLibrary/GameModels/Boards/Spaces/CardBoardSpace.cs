using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Cards;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.Boards.Spaces;

public class CardBoardSpace : DataModel, IBoardSpace
{
    public int Id { get; set; }
    public uint BoardIndex { get; set; }
    public string Name { get; set; }
    public BoardSpaceType SpaceType { get; set; } = BoardSpaceType.Card;
    
    public int BoardId { get; set; }
    [ForeignKey(nameof(BoardId))]
    public virtual Board Board { get; set; }
    
    public int CardTypeId { get; set; }
    [ForeignKey(nameof(CardTypeId))]
    public virtual CardType CardType { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
        if (SpaceType != BoardSpaceType.Card)
        {
            throw new InvalidOperationException("SpaceType must be Card for CardBoardSpace.");
        }

        if (string.IsNullOrEmpty(Name))
        {
            modelState.AddModelError("Name", "Name cannot be empty.");
        }
        
        if(BoardId <= 0)
        {
            modelState.AddModelError("BoardId", "You must select a board.");
        }
        
        if(CardTypeId <= 0)
        {
            modelState.AddModelError("CardTypeId", "You must select a card type.");
        }
    }
}