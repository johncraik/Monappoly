using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.MiscGameObjs;

public class DiceDataModel : DataModel
{
    public int Id { get; set; }
    public uint Sides { get; set; }
    public uint ThirdDiceSides { get; set; }

    public void Validate(ModelStateDictionary modelState)
    {
        if (Sides < 6)
        {
            modelState.AddModelError(nameof(Sides), "You must have at least 6 sides on the dice.");
        }
        
        if (ThirdDiceSides < 6)
        {
            modelState.AddModelError(nameof(ThirdDiceSides), "You must have at least 6 sides on the dice.");
        }
    }
}