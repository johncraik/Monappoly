using Microsoft.AspNetCore.Identity;

namespace Monappoly_ASP.Authentication.User;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public int TenantId { get; set; }
}
