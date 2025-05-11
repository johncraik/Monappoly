using Microsoft.AspNetCore.Mvc;
using Monappoly_ASP.Areas.Admin.Services;

namespace Monappoly_ASP.Areas.Admin.Controllers;

public class UserController : Controller
{
    private readonly AdminService _adminService;

    public UserController(AdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost]
    public async Task<bool> ChangeUserName(string userId, string username) =>
        await _adminService.UpdateUserName(userId, username);
    
    [HttpPost]
    public async Task<bool> ChangeUserDisplayName(string userId, string displayName) =>
        await _adminService.UpdateUserDisplayName(userId, displayName);
    
    [HttpPost]
    public async Task<bool> ChangeUserEmail(string userId, string email) =>
        await _adminService.UpdateUserEmail(userId, email);
    
    [HttpPost]
    public async Task<bool> ChangeUserPhoneNumber(string userId, string phone) =>
        await _adminService.UpdateUserPhoneNumber(userId, phone);
}