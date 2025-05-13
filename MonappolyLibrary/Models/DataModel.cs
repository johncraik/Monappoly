namespace MonappolyLibrary.Models;

public class DataModel
{
    public int TenantId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsModifiable() => TenantId > 0;
    public bool IsDeletable() => TenantId > 0;
    
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }

    internal void FillCreated()
    {
        CreatedBy = "System";
        CreatedDate = DateTime.UtcNow;
    }
    public void FillCreated(UserInfo userInfo)
    {
        CreatedBy = userInfo.UserId;
        CreatedDate = DateTime.UtcNow;
    }
    
    internal bool FillModified()
    {
        if(!IsModifiable()) return false;
        
        ModifiedBy = "System";
        ModifiedDate = DateTime.UtcNow;
        return true;
    }
    internal void ForceModify()
    {
        ModifiedBy = "System";
        ModifiedDate = DateTime.UtcNow;
    }
    public bool FillModified(UserInfo userInfo)
    {
        if(!IsModifiable()) return false;

        ModifiedBy = userInfo.UserId;
        ModifiedDate = DateTime.UtcNow;
        return true;
    }
    
    internal bool FillDeleted()
    {
        if(!IsDeletable()) return false;
        
        DeletedBy = "System";
        DeletedDate = DateTime.UtcNow;
        IsDeleted = true;

        return true;
    }
    internal void ForceDelete()
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