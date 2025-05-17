using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MonappolyLibrary.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum e)
        => Regex.Replace(e.ToString(), "(?<!^)([A-Z])", " $1");
    
    public static List<SelectListItem> GetSelectList(this Enum e, bool insertSelect = false, string selectText = "Select an Item")
    {
        var vals = Enum.GetValues(e.GetType()).Cast<Enum>();
        
        var list = vals.Where(val => val.GetHashCode() >= 0).OrderBy(val => val.GetHashCode())
            .Select(val => new SelectListItem
            {
                Text = val.GetDisplayName(), 
                Value = val.GetHashCode().ToString()
            }).ToList();

        if (!insertSelect) return list;
        
        list.Insert(0, new SelectListItem
        {
            Text = selectText,
            Value = "-1"
        });
        return list;
    }
}