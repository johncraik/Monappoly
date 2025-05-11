namespace MonappolyLibrary.Models;

public class AuditRecord
{
    public string EntityName { get; set; }
    public string Action { get; set; }
    public DateTime? Date { get; set; }
}