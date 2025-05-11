using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Monappoly_ASP.Areas.Admin.Models;
using Monappoly_ASP.Areas.Admin.Services;
using Monappoly_ASP.Authentication.Tenant;
using Monappoly_ASP.Authentication.User;

namespace Monappoly_ASP.Areas.Admin.Pages;

[Authorize(Roles = $"{UserRoles.TenantAdmin}, {UserRoles.ServerAdmin}")]
public class Index : PageModel
{
    private readonly AdminService _adminService;

    public Index(AdminService adminService)
    {
        _adminService = adminService;
    }

    public Tenant Tenant {get; set;}
    public List<UserWithRoles> Users {get; set;}
    
    public async Task<bool> SetupPage()
    {
        var t = await _adminService.FindTenant();
        if (t == null) return false;
        
        Tenant = t;
        Users = await _adminService.GetUsersInTenant(t.Id);
        return true;
    }
    
    public async Task<IActionResult> OnGet()
    {
        var res = await SetupPage();
        return res ? Page() : RedirectToPage($"/{nameof(Index)}");
    }
}