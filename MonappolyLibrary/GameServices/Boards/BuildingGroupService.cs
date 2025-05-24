using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;
using MonappolyLibrary.GameModels.MiscGameObjs;
using MonappolyLibrary.GameModels.MiscGameObjs.ViewModels;

namespace MonappolyLibrary.GameServices.Boards;

public class BuildingGroupService
{
    private readonly MonappolyDbContext _context;

    public BuildingGroupService(MonappolyDbContext context)
    {
        _context = context;
    }

    public async Task<List<SelectListItem>> GetBuildingGroupsDropdown() =>
        await _context.BuildingGroups.Select(bg => new SelectListItem
        {
            Text = bg.Name,
            Value = bg.Id.ToString()
        }).ToListAsync();

    public async Task<List<BuildingPoolViewModel>> GetBuildings(int buildingGroupId)
    {
        var pools = await _context.BuildingPools.Where(p => p.BuildingGroupId == buildingGroupId)
            .Include(p => p.BuildingGroup)
            .ToListAsync();

        var poolList = new List<BuildingPoolViewModel>();
        foreach (var p in pools)
        {
            var buildings = await _context.Buildings.Where(b => b.BuildingPoolId == p.Id)
                .ToListAsync();
            poolList.Add(new BuildingPoolViewModel
            {
                Pool = p,
                Buildings = buildings
            });
        }
        return poolList;
    }
}