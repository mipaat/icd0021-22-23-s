using AutoMapper;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseDbSetTrackingMapper<TDomainEntity, TDalEntity> :
    BaseDbSetTrackingMapper<TDomainEntity, TDalEntity, Guid>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TDalEntity : class, IIdDatabaseEntity<Guid>
{
    public BaseDbSetTrackingMapper(IMapper mapper, DbSet<TDomainEntity> dbSet) : base(mapper, dbSet)
    {
    }
}

public class BaseDbSetTrackingMapper<TDomainEntity, TDalEntity, TKey> :
    AbstractBaseTrackingMapper<TDomainEntity, TDalEntity, TKey>
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
    where TDalEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    private readonly DbSet<TDomainEntity> _dbSet;

    public BaseDbSetTrackingMapper(IMapper mapper, DbSet<TDomainEntity> dbSet) : base(mapper)
    {
        _dbSet = dbSet;
    }

    protected override TDomainEntity? GetTrackedSourceEntity(TKey key)
    {
        return _dbSet.Local.FirstOrDefault(e => e.Id.Equals(key));
    }

    protected override void SetTrackedSourceEntity(TKey key, TDomainEntity entity)
    {
        // Don't mess with the DbSet
        // If an entity isn't tracked by the DbSet, it doesn't matter if it's re-mapped as a new object
    }
}