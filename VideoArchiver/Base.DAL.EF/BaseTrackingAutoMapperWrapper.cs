using AutoMapper;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseTrackingAutoMapperWrapper<TDbContext> : BaseTrackingAutoMapperWrapper<TDbContext, Guid> where TDbContext : DbContext
{
    public BaseTrackingAutoMapperWrapper(IMapper mapper, TDbContext dbContext) : base(mapper, dbContext)
    {
    }
}

public class BaseTrackingAutoMapperWrapper<TDbContext, TKey> : AbstractTrackingAutoMapperWrapper<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public BaseTrackingAutoMapperWrapper(IMapper mapper, TDbContext dbContext) : base(mapper)
    {
        _dbContext = dbContext;
    }

    protected override TEntity? GetTrackedEntityOrDefault<TEntity>(TEntity entity) where TEntity : class
    {
        if (!IsIdDatabaseEntity(typeof(TEntity))) return null;
        var idDatabaseEntity = (IIdDatabaseEntity<TKey>)entity;
        return _dbContext.ChangeTracker.Entries<TEntity>()
            .SingleOrDefault(e => ((IIdDatabaseEntity<TKey>)e.Entity).Id.Equals(idDatabaseEntity.Id))
            ?.Entity;
    }

    protected override void SetTrackedEntity<TEntity>(TEntity entity)
    {
        // Don't mess with the DbSet
        // If an entity isn't tracked by the DbSet, it doesn't matter if it's re-mapped as a new object
    }
}