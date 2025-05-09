namespace MonappolyLibrary;

public class UserInfo
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public int TenantId { get; set; }

    public bool IsSetup { get; set; } = false;
}