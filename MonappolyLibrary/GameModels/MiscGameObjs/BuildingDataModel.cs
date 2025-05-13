using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.MiscGameObjs;

public class BuildingDataModel : DataModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public uint Count { get; set; }
    
    public BuildingRule BuildingRule { get; set; }
    public BuildOnRule BuildOnRule { get; set; }
    public RentRule RentRule { get; set; }
    public uint RentMultiplier { get; set; }
    
    public int BuildingGroupId { get; set; }
    [ForeignKey(nameof(BuildingGroupId))]
    public virtual BuildingGroup BuildingGroup { get; set; }
}

public enum RentRule
{
    Standard = -1,
    HouseMultiplied,
    HotelMultiplied
}

public enum BuildingRule
{
    Standard = -1,
    None,
    BeforeHouse,
    AfterHotel
}

[Flags]
public enum BuildOnRule
{
    Standard = -1,
    OnMortgaged,
    OnStation,
    OnUtility,
    OnTax,
    OnCard,
    OnJail,
    OnFreeParking
}