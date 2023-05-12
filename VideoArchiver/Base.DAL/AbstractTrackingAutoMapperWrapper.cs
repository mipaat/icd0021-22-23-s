using AutoMapper;
using Contracts.DAL;
using Domain.Base;

namespace Base.DAL;

public abstract class AbstractTrackingAutoMapperWrapper : AbstractTrackingAutoMapperWrapper<Guid>
{
    protected AbstractTrackingAutoMapperWrapper(IMapper mapper) : base(mapper)
    {
    }
}

public abstract class AbstractTrackingAutoMapperWrapper<TKey> : ITrackingAutoMapperWrapper
    where TKey : struct, IEquatable<TKey>
{
    private readonly IMapper _mapper;

    protected AbstractTrackingAutoMapperWrapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected abstract TEntity? GetTrackedEntityOrDefault<TEntity>(TEntity entity) where TEntity : class;

    protected abstract void SetTrackedEntity<TEntity>(TEntity entity) where TEntity : class;

    protected static bool IsIdDatabaseEntity(Type type)
    {
        return type.GetInterfaces().Contains(typeof(IIdDatabaseEntity<TKey>));
    }

    public TDestination? Map<TSource, TDestination>(TSource? source)
        where TSource : class
        where TDestination : class
    {
        if (source == null) return null;
        var trackedSource = GetTrackedEntityOrDefault(source);
        if (trackedSource == null)
        {
            SetTrackedEntity(source);
            trackedSource = source;
        }

        var dest = _mapper.Map<TSource, TDestination>(trackedSource);

        var trackedDest = GetTrackedEntityOrDefault(dest);
        if (trackedDest == null)
        {
            SetTrackedEntity(dest);
            trackedDest = dest;
        }

        return _mapper.Map(trackedSource, trackedDest);
    }

    public TSource Map<TSource, TDestination>(TDestination source, TSource destination)
    {
        return _mapper.Map(source, destination);
    }
}