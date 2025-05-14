using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MonappolyLibrary.GameModels.Cards.ViewModels;

public class CardUploadInputModel
{
    [Required]
    [DisplayName("Card Deck")]
    public int CardDeckId { get; set; }
    [Required]
    [DisplayName("Card Type")]
    public int CardTypeId { get; set; }
    [Required]
    [DisplayName("Cards")]
    public IFormFile UploadFile { get; set; }
}