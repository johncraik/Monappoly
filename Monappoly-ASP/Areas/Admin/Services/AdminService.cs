using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Monappoly_ASP.Areas.Admin.Models;
using Monappoly_ASP.Authentication.Tenant;
using Monappoly_ASP.Authentication.User;
using Monappoly_ASP.Data;
using MonappolyLibrary;
using MonappolyLibrary.Data;
using MonappolyLibrary.Models;
using MonappolyLibrary.Services;

namespace Monappoly_ASP.Areas.Admin.Services;

public class AdminService
{
    private readonly ApplicationDbContext _context;
    private readonly MonappolyDbContext _gameContext;
    private readonly UserInfo _userInfo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminService(ApplicationDbContext context,
        MonappolyDbContext gameContext,
        UserInfo userInfo,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _gameContext = gameContext;
        _userInfo = userInfo;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<Tenant>> GetTenants()
        => await _context.Tenants.Where(t => !t.IsDeleted).ToListAsync();

    public async Task<Tenant?> FindTenant()
        => await _context.Tenants.FirstOrDefaultAsync(t => t.Id == _userInfo.TenantId && !t.IsDeleted);

    public async Task<List<UserWithRoles>> GetUsersInTenant(int tenantId)
    {
        var users = await _context.Users.Where(u => u.TenantId == tenantId).ToListAsync();
        var userList = new List<UserWithRoles>();
        foreach (var user in users)
        {
            var roleIds = _context.UserRoles.Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId).AsEnumerable();
            var roles = await _context.Roles.Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name).ToListAsync();
            userList.Add(new UserWithRoles
            {
                User = user,
                Roles = roles.ToList()
            });
        }

        return userList;
    }

    public async Task<List<SelectListItem>> GetRoleList(ApplicationUser user, bool isInRole)
    {
        var roles = typeof(UserRoles)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(fi => fi.GetRawConstantValue()?.ToString())
            .ToList();
        var userRoleIds = _context.UserRoles.Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.RoleId).AsEnumerable();
        var userRoles = await _context.Roles.Where(r => userRoleIds.Contains(r.Id))
            .Select(r => r.Name).ToListAsync();
        
        List<SelectListItem> list;
        if (isInRole)
        {
            list = roles.Where(r => userRoles.Contains(r))
                .Select(r => new SelectListItem
                {
                    Text = r,
                    Value = r
                }).ToList();
        }
        else
        {
            list = roles.Where(r => !userRoles.Contains(r))
                .Select(r => new SelectListItem
                {
                    Text = r,
                    Value = r
                }).ToList();
        }

        return list;
    }

    public async Task<List<TenantListViewModel>> GetTenantList()
    {
        var tenants = await GetTenants();
        var tenantList = new List<TenantListViewModel>();
        foreach (var t in tenants)
        {
            var vm = new TenantListViewModel
            {
                Tenant = t
            };
            var users = await GetUsersInTenant(t.Id);
            vm.Users = users;
            
            tenantList.Add(vm);
        }

        return tenantList;
    }
    
    

    #region User Managment

    public async Task<ApplicationUser?> GetUser(string userId)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    
    public async Task<Tenant> GetUserTenant(int tenantId)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
        if (tenant != null) return tenant;

        return new Tenant
        {
            Id = -1,
            TenantName = "No Tenant Found",
            DateCreated = DateTime.UtcNow
        };
    }

    public async Task<bool> UpdateUserName(string userId, string username)
    {
        var user = await GetUser(userId);
        if(user == null) return false;

        var existingUser = await _userManager.FindByNameAsync(username);
        if(existingUser != null) return false;

        var res = await _userManager.SetUserNameAsync(user, username);
        return res.Succeeded;
    }
    
    public async Task<bool> UpdateUserEmail(string userId, string email)
    {
        var user = await GetUser(userId);
        if(user == null) return false;

        var existingUser = await _userManager.FindByEmailAsync(email);
        if(existingUser != null) return false;

        var res = await _userManager.SetEmailAsync(user, email);
        return res.Succeeded;
    }
    
    public async Task<bool> UpdateUserPhoneNumber(string userId, string phoneNumber)
    {
        var user = await GetUser(userId);
        if(user == null) return false;

        var res = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
        return res.Succeeded;
    }

    public async Task<bool> UpdateUserDisplayName(string userId, string displayName)
    {
        var user = await GetUser(userId);
        if(user == null) return false;
        
        user.DisplayName = displayName;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeUserTenant(string userId, int tenantId)
    {
        var user = await GetUser(userId);
        if (user == null) return false;
        
        var validTenant = await _context.Tenants.AnyAsync(t => t.Id == tenantId && !t.IsDeleted);
        if(!validTenant) return false;
        
        user.TenantId = tenantId;
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    public async Task<bool> ResetUserPassword(ApplicationUser user, string newPassword)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var res = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return res.Succeeded;
    }
    
    public async Task<bool> AddUserToRole(ApplicationUser user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role)) return false;
        
        var res = await _userManager.AddToRoleAsync(user, role);
        return res.Succeeded;
    }
    
    public async Task<bool> RemoveUserFromRole(ApplicationUser user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role)) return false;
        
        var res = await _userManager.RemoveFromRoleAsync(user, role);
        return res.Succeeded;
    }
    
    public async Task<bool> DeleteUser(ApplicationUser user)
    {
        var res = await _userManager.DeleteAsync(user);
        return res.Succeeded;
    }

    public List<AuditRecord> GetAuditRecords(string userId) => 
        AuditHelper.GetAuditTrailForUser(_gameContext, userId)
            .OrderByDescending(a => a.Date).ToList();

    #endregion


    #region Tenant Management

    public async Task<bool> CreateNewTenant(string name)
    {
        var existing =
            await _context.Tenants.AnyAsync(t => t.TenantName.ToLower() == name.ToLower() && !t.IsDeleted);
        
        if(existing) return false;
        
        var tenant = new Tenant
        {
            TenantName = name,
            DateCreated = DateTime.UtcNow,
            IsDeleted = false
        };

        await _context.Tenants.AddAsync(tenant);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion
}