using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Monappoly_ASP.Areas.Admin.Services;
using Monappoly_ASP.Authentication.User;

namespace Monappoly_ASP.Areas.Admin.Pages.Tenants;

[Authorize(Roles = UserRoles.ServerAdmin)]
public class New : PageModel
{
    private readonly AdminService _adminService;

    public New(AdminService adminService)
    {
        _adminService = adminService;
    }
    
    [BindProperty]
    [Required]
    public string TenantName { get; set; }

    public void OnGet()
    {
        // This method is intentionally left empty.
        // The page will be displayed with the form for creating a new tenant.
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Create a new tenant using the provided name
        var result = await _adminService.CreateNewTenant(TenantName);

        if (result) return RedirectToPage($"/Tenants/{nameof(Index)}", new {area = "Admin"});
        
        // If there was an error, show the form again with an error message
        ModelState.AddModelError(nameof(TenantName), "A tenant with this name already exists.");
        return Page();
    }
}