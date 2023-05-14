using Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Base.DAL.EF;

public static class ChangeTrackerExtensions
{
    public static TEntity? GetEntity<TEntity>(this ChangeTracker changeTracker, Guid id)
        where TEntity : class, IIdDatabaseEntity => GetEntity<TEntity, Guid>(changeTracker, id);

    public static TEntity? GetEntity<TEntity, TKey>(this ChangeTracker changeTracker, TKey id)
        where TEntity : class, IIdDatabaseEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        return changeTracker.Entries<TEntity>().GetEntity(id);
    }

    public static TEntity? GetEntity<TEntity>(this IEnumerable<EntityEntry<TEntity>> entries, Guid id)
        where TEntity : class, IIdDatabaseEntity =>
        GetEntity<TEntity, Guid>(entries, id);

    public static TEntity? GetEntity<TEntity, TKey>(this IEnumerable<EntityEntry<TEntity>> entries, TKey id)
        where TEntity : class, IIdDatabaseEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        return entries.FirstOrDefault(e => e.Entity.GetType() == typeof(TEntity) && e.Entity.Id.Equals(id))?.Entity;
    }

    public static TDomainEntity? GetTrackedEntity<TDomainEntity, TKey>(this DbSet<TDomainEntity> dbSet, TKey id)
        where TDomainEntity : class, IIdDatabaseEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        return dbSet.Local.FirstOrDefault(e => e.Id.Equals(id));
    }

    public static TDomainEntity? GetTrackedEntity<TDomainEntity, TKey>(this DbContext dbContext, TKey id)
        where TDomainEntity : class, IIdDatabaseEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        var previousAutoDetect = dbContext.ChangeTracker.AutoDetectChangesEnabled;
        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        var result = dbContext.ChangeTracker.Entries<TDomainEntity>().FirstOrDefault(e => e.Entity.Id.Equals(id))?.Entity;
        dbContext.ChangeTracker.AutoDetectChangesEnabled = previousAutoDetect;
        return result;
    }

    public static TDomainEntity? GetTrackedEntity<TDomainEntity>(this DbContext dbContext, Guid id)
        where TDomainEntity : class, IIdDatabaseEntity => GetTrackedEntity<TDomainEntity, Guid>(dbContext, id);
}