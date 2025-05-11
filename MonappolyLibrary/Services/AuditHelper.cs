using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.Services;

public static class AuditHelper
{
    public static List<AuditRecord> GetAuditTrailForUser(MonappolyDbContext dbContext, string userId)
    {
        var auditRecords = new List<AuditRecord>();

        var dbSetProperties = dbContext.GetType()
            .GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

        foreach (var dbSetProp in dbSetProperties)
        {
            var entityType = dbSetProp.PropertyType.GenericTypeArguments.First();
            if (!typeof(DataModel).IsAssignableFrom(entityType))
                continue;

            var dbSet = dbSetProp.GetValue(dbContext);
            if (dbSet is not IQueryable data) continue;

            foreach (var item in data)
            {
                var model = item as DataModel;
                if (model == null) continue;

                if (model.CreatedBy == userId)
                {
                    auditRecords.Add(new AuditRecord
                    {
                        EntityName = entityType.Name,
                        Action = "Created",
                        Date = model.CreatedDate
                    });
                }

                if (model.ModifiedBy == userId)
                {
                    auditRecords.Add(new AuditRecord
                    {
                        EntityName = entityType.Name,
                        Action = "Modified",
                        Date = model.ModifiedDate
                    });
                }

                if (model.DeletedBy == userId)
                {
                    auditRecords.Add(new AuditRecord
                    {
                        EntityName = entityType.Name,
                        Action = "Deleted",
                        Date = model.DeletedDate
                    });
                }
            }
        }

        return auditRecords;
    }
}