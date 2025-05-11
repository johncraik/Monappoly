using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Monappoly_ASP.Areas.Admin.Models;
using Monappoly_ASP.Areas.Admin.Services;
using Monappoly_ASP.Authentication.User;

namespace Monappoly_ASP.Areas.Admin.Pages.Tenants;

[Authorize(Roles = UserRoles.ServerAdmin)]
public class Index : PageModel
{
    private readonly AdminService _adminService;

    public Index(AdminService adminService)
    {
        _adminService = adminService;
    }

    public List<TenantListViewModel> TenantList { get; set; }
    
    public async Task SetupPage()
    {
        TenantList = await _adminService.GetTenantList();
    }
    
    public async Task<IActionResult> OnGet()
    {
        await SetupPage();
        return Page();
    }
}