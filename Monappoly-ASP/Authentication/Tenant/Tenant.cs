namespace Monappoly_ASP.Authentication.Tenant;

public class Tenant
{
    public int Id { get; set; }
    public required string TenantName { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DateDeleted { get; set; }
}