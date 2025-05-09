using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Monappoly_ASP.Authentication.User.UserClaims;

public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var defaultClaims = await base.GenerateClaimsAsync(user);
        
        defaultClaims.AddClaim((new Claim(UserClaims.DisplayNameClaim, user.DisplayName ?? "")));
        defaultClaims.AddClaim((new Claim(UserClaims.TenantId, user.TenantId.ToString())));

        return defaultClaims;
    }

    public UserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
        : base (userManager, roleManager, options)
    {
    }
}