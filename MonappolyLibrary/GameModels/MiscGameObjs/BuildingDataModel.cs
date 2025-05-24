using System.ComponentModel.DataAnnotations.Schema;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.GameModels.MiscGameObjs;

public class BuildingDataModel : DataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public BuildingRule BuildingRule { get; set; }
    public BuildOnRule BuildOnRule { get; set; }
    public RentRule RentRule { get; set; }
    public uint RentMultiplier { get; set; }
    public uint BuildingCostMultiplier { get; set; }
    public BuildingCap CapType { get; set; }
    public uint BuildingCap { get; set; }
    
    public int BuildingPoolId { get; set; }
    [ForeignKey(nameof(BuildingPoolId))]
    public virtual BuildingPool BuildingPool { get; set; }
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

public enum BuildingCap
{
    None,
    PerSet,
    Total
}