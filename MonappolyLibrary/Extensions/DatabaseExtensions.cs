using Microsoft.EntityFrameworkCore;
using MonappolyLibrary.Data.Defaults.Dictionaries;
using MonappolyLibrary.Models;

namespace MonappolyLibrary.Extensions;

public static class DatabaseExtensions
{
    public static IQueryable<T> OnlyDeleted<T>(this IQueryable<T> query, int tenantId) 
        where T : DataModel
    {
        return query.IgnoreQueryFilters()
            .Where(e => (e.TenantId == tenantId || e.TenantId < 0) && e.IsDeleted);
    }
    
    public static IQueryable<T> IncludeDeleted<T>(this IQueryable<T> query, int tenantId) 
        where T : DataModel
    {
        return query.IgnoreQueryFilters()
            .Where(e => e.TenantId == tenantId || e.TenantId < 0);
    }
    
    public static IQueryable<T> AllTenants<T>(this IQueryable<T> query) 
        where T : DataModel
    {
        return query.IgnoreQueryFilters();
    }
    
    public static IQueryable<T> AllTenants<T>(this IQueryable<T> query, bool deleted) 
        where T : DataModel
    {
        return query.IgnoreQueryFilters().Where(e => e.IsDeleted == deleted);
    }
    
    public static IQueryable<T> MonopolyDefaults<T>(this IQueryable<T> query) 
        where T : DataModel
    {
        return query.IgnoreQueryFilters().Where(e => e.TenantId == DefaultsDictionary.MonopTenant);
    }
}