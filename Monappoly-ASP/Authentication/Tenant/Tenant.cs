using System.ComponentModel;

namespace Monappoly_ASP.Authentication.Tenant;

public class Tenant
{
    public int Id { get; set; }
    [DisplayName("Tenant Name")]
    public required string TenantName { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DateDeleted { get; set; }
}