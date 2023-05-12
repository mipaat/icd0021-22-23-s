using AutoMapper;
using Domain.Base;

namespace Base.DAL;

public class BaseDictionaryTrackingMapper<TSource, TDestination> :
    BaseDictionaryTrackingMapper<TSource, TDestination, Guid>
    where TSource : class, IIdDatabaseEntity<Guid>
    where TDestination : class, IIdDatabaseEntity<Guid>
{
    public BaseDictionaryTrackingMapper(IMapper mapper) : base(mapper)
    {
    }
}

public class BaseDictionaryTrackingMapper<TSource, TDestination, TKey> :
    AbstractBaseTrackingMapper<TSource, TDestination, TKey>
    where TKey : struct, IEquatable<TKey>
    where TSource : class, IIdDatabaseEntity<TKey>
    where TDestination : class, IIdDatabaseEntity<TKey>
{
    public BaseDictionaryTrackingMapper(IMapper mapper) : base(mapper)
    {
    }

    private readonly Dictionary<TKey, TSource> _trackedSourceEntities = new();

    protected override TSource? GetTrackedSourceEntity(TKey key)
    {
        return _trackedSourceEntities.GetValueOrDefault(key);
    }

    protected override void SetTrackedSourceEntity(TKey key, TSource entity)
    {
        _trackedSourceEntities[key] = entity;
    }
}