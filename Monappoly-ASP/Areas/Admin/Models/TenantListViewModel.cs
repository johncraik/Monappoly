using Monappoly_ASP.Authentication.Tenant;
using Monappoly_ASP.Authentication.User;

namespace Monappoly_ASP.Areas.Admin.Models;

public class TenantListViewModel
{
    public Tenant Tenant { get; set; }
    public List<UserWithRoles> Users { get; set; }
}