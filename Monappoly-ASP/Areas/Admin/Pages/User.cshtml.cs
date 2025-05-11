using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monappoly_ASP.Areas.Admin.Services;
using Monappoly_ASP.Authentication.Tenant;
using Monappoly_ASP.Authentication.User;
using MonappolyLibrary;
using MonappolyLibrary.Models;

namespace Monappoly_ASP.Areas.Admin.Pages;

[Authorize(Roles = $"{UserRoles.TenantAdmin}, {UserRoles.ServerAdmin}")]

public class User : PageModel
{
    private readonly AdminService _adminService;
    private readonly UserInfo _userInfo;
    public const string PasswordReset = "Monappoly_Password23";

    public User(AdminService adminService, UserInfo userInfo)
    {
        _adminService = adminService;
        _userInfo = userInfo;
    }
    
    public Tenant Tenant { get; set; }
    public ApplicationUser ManagedUser { get; set; }
    public List<SelectListItem> AddRoles { get; set; }
    public List<SelectListItem> RemoveRoles { get; set; }
    public List<AuditRecord> AuditRecords { get; set; }
    
    public string? ErrorMessage { get; set; }
    public string? Message { get; set; }
    
    [BindProperty]
    public string TenantId { get; set; }
    
    [BindProperty]
    [DisplayName("Selected Role")]
    public string SelectedRole { get; set; }
    
    public async Task<IActionResult?> Setup(string id)
    {
        var user = await _adminService.GetUser(id);
        if(user == null) return new NotFoundResult();
        
        if (!User.IsInRole(UserRoles.ServerAdmin)
            && user.TenantId != _userInfo.TenantId)
            return new UnauthorizedResult();
        
        ManagedUser = user;
        Tenant = await _adminService.GetUserTenant(user.TenantId);
        AddRoles = await _adminService.GetRoleList(user, false);
        RemoveRoles = await _adminService.GetRoleList(user, true);
        AuditRecords = _adminService.GetAuditRecords(id);
        return null;
    }
    
    public async Task<IActionResult> OnGet(string id)
    {
        var res = await Setup(id);
        if(res != null) return res;
        
        Message = TempData["SuccessMessage"] as string;
        return Page();
    }
    
    public async Task<IActionResult> OnPostTenant(string id)
    {
        var res = await Setup(id);
        if(res != null) return res;

        var success = int.TryParse(TenantId, out var tid);
        if (success)
        {
            var result = await _adminService.ChangeUserTenant(id, tid);
            if (result)
            {
                TempData["SuccessMessage"] = $"Tenant ID changed to {TenantId}.";
                return RedirectToPage();
            }
        }
        
        ErrorMessage = $"Failed to change tenant ID to {TenantId}. Ensure this ID is a valid tenant.";
        return Page();
    }

    public async Task<IActionResult> OnPostResetPassword(string id)
    {
        var res = await Setup(id);
        if(res != null) return res;

        var result = await _adminService.ResetUserPassword(ManagedUser, PasswordReset);
        if (result)
        {
            TempData["SuccessMessage"] = $"Password reset to {PasswordReset}.";
            return RedirectToPage();
        }
        
        ErrorMessage = "Failed to reset password.";
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteUser(string id)
    {
        var res = await Setup(id);
        if(res != null) return res;

        var result = await _adminService.DeleteUser(ManagedUser);
        if (result)
            return RedirectToPage($"/{nameof(Index)}", new {area = "Admin"});
        
        ErrorMessage = "Failed to delete user.";
        return Page();
    }
    
    public async Task<IActionResult> OnPostAddRole(string id)
    {
        var res = await Setup(id);
        if(res != null) return res;

        var result = await _adminService.AddUserToRole(ManagedUser, SelectedRole);
        if (result)
        {
            TempData["SuccessMessage"] = $"Role {SelectedRole} added.";
            return RedirectToPage();
        }
        
        ErrorMessage = $"Failed to add role {SelectedRole}.";
        return Page();
    }
    
    public async Task<IActionResult> OnPostRemoveRole(string id)
    {
        var res = await Setup(id);
        if(res != null) return res;

        var result = await _adminService.RemoveUserFromRole(ManagedUser, SelectedRole);
        if (result)
        {
            TempData["SuccessMessage"] = $"Role {SelectedRole} removed.";
            return RedirectToPage();
        }
        
        ErrorMessage = $"Failed to remove role {SelectedRole}.";
        return Page();
    }
}