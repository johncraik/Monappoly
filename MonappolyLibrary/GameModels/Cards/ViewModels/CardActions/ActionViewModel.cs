using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MonappolyLibrary.GameModels.Cards.CardActions;

namespace MonappolyLibrary.GameModels.Cards.ViewModels.CardActions;

public class ActionViewModel : ICardAction
{
    private const string DescOpenWrapper = "<div class=\"mb-1\"><label><b>";
    private const string DescCentreWrapper = " </b>";
    private const string DescCloseWrapper = "</label></div>";
    
    public int Id { get; set; }
    public int GroupId { get; set; }
    [DisplayName("Action Type")]
    public ActionType Type { get; set; }
    public uint TurnLength { get; set; }
    
    [DisplayName("Action Description")]
    public string ActionDescription { get; set; }
    
    public void Validate(ModelStateDictionary modelState)
    {
    }

    public ActionViewModel ToViewModel()
    {
        return this;
    }

    public ActionViewModel(ICardAction action, params (string Key, string Value, bool? Condition)[] properties)
    {
        Id = action.Id;
        GroupId = action.GroupId;
        Type = action.Type;
        TurnLength = action.TurnLength;

        foreach (var prop in properties)
        {
            if (prop.Condition ?? true)
            {
                ActionDescription += $"{DescOpenWrapper}{prop.Key}{DescCentreWrapper}{prop.Value}{DescCloseWrapper}";
            }
        }
    }
}