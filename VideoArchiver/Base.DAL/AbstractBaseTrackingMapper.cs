using AutoMapper;
using Domain.Base;

namespace Base.DAL;

public abstract class AbstractBaseTrackingMapper<TSource, TDestination> :
    AbstractBaseTrackingMapper<TSource, TDestination, Guid>
    where TSource : class, IIdDatabaseEntity<Guid>
    where TDestination : class, IIdDatabaseEntity<Guid>
{
    protected AbstractBaseTrackingMapper(IMapper mapper) : base(mapper)
    {
    }
}

public abstract class AbstractBaseTrackingMapper<TSource, TDestination, TKey> : BaseMapper<TSource, TDestination>
    where TSource : class, IIdDatabaseEntity<TKey>
    where TDestination : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected abstract TSource? GetTrackedSourceEntity(TKey key);
    protected abstract void SetTrackedSourceEntity(TKey key, TSource entity);

    protected AbstractBaseTrackingMapper(IMapper mapper) : base(mapper)
    {
    }

    public override TSource? Map(TDestination? entity)
    {
        if (entity == null) return null;

        var trackedSourceEntity = GetTrackedSourceEntity(entity.Id);
        if (trackedSourceEntity == null)
        {
            trackedSourceEntity = base.Map(entity)!;
            SetTrackedSourceEntity(entity.Id, trackedSourceEntity);
        }
        else
        {
            Map(entity, trackedSourceEntity);
        }

        return trackedSourceEntity;
    }
}