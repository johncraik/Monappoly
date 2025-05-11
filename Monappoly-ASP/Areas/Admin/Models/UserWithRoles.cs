using Monappoly_ASP.Authentication.User;

namespace Monappoly_ASP.Areas.Admin.Models;

public class UserWithRoles
{
    public ApplicationUser User { get; set; }
    public List<string?> Roles { get; set; }
}