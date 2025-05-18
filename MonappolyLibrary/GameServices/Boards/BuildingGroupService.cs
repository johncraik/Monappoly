using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;

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
}