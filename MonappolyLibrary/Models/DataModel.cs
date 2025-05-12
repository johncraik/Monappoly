namespace MonappolyLibrary.Models;

public class DataModel
{
    public int TenantId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsDeletable() => TenantId > 0;
    
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }

    public void FillCreated()
    {
        CreatedBy = "System";
        CreatedDate = DateTime.UtcNow;
    }
    public void FillCreated(UserInfo userInfo)
    {
        CreatedBy = userInfo.UserId;
        CreatedDate = DateTime.UtcNow;
    }
    
    public void FillModified()
    {
        ModifiedBy = "System";
        ModifiedDate = DateTime.UtcNow;
    }
    public void FillModified(UserInfo userInfo)
    {
        ModifiedBy = userInfo.UserId;
        ModifiedDate = DateTime.UtcNow;
    }
    
    public bool FillDeleted()
    {
        if(!IsDeletable()) return false;
        
        DeletedBy = "System";
        DeletedDate = DateTime.UtcNow;
        IsDeleted = true;

        return true;
    }
    public void ForceDelete()
    {
        DeletedBy = "System";
        DeletedDate = DateTime.UtcNow;
        IsDeleted = true;
    }
    public bool FillDeleted(UserInfo userInfo)
    {
        if(!IsDeletable()) return false;
        
        DeletedBy = userInfo.UserId;
        DeletedDate = DateTime.UtcNow;
        IsDeleted = true;

        return true;
    }
}