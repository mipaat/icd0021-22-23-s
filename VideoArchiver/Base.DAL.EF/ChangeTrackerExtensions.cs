using Domain.Base;
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
}