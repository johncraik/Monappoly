namespace MonappolyLibrary.Models;

public class DataModel
{
    public int TenantId { get; set; }
    public bool IsDeleted { get; set; }
    
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }

    public void FillCreated(UserInfo userInfo)
    {
        CreatedBy = userInfo.UserId;
        CreatedDate = DateTime.UtcNow;
    }
    
    public void FillDeleted(UserInfo userInfo)
    {
        DeletedBy = userInfo.UserId;
        DeletedDate = DateTime.UtcNow;
        IsDeleted = true;
    }
    
    public void FillModified(UserInfo userInfo)
    {
        ModifiedBy = userInfo.UserId;
        ModifiedDate = DateTime.UtcNow;
    }
}