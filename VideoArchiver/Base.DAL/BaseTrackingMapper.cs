using Contracts.DAL;

namespace Base.DAL;

public class BaseTrackingMapperUnidirectional<TSource, TDestination> :
    BaseTrackingMapperUnidirectional<TSource, TDestination, Guid> where TSource : class where TDestination : class
{
    public BaseTrackingMapperUnidirectional(ITrackingAutoMapperWrapper mapper) : base(mapper)
    {
    }
}

public class BaseTrackingMapperUnidirectional<TSource, TDestination, TKey> :
    IMapperUnidirectional<TSource, TDestination>
    where TKey : struct, IEquatable<TKey>
    where TSource : class
    where TDestination : class
{
    protected readonly ITrackingAutoMapperWrapper Mapper;

    public BaseTrackingMapperUnidirectional(ITrackingAutoMapperWrapper mapper)
    {
        Mapper = mapper;
    }

    public TDestination? Map(TSource? entity)
    {
        return Mapper.Map<TSource, TDestination>(entity);
    }

    public TDestination Map(TSource source, TDestination destination)
    {
        return Mapper.Map(source, destination);
    }
}

public class BaseTrackingMapper<TSource, TDestination> :
    BaseTrackingMapper<TSource, TDestination, Guid>
    where TSource : class
    where TDestination : class
{
    public BaseTrackingMapper(ITrackingAutoMapperWrapper mapper) : base(mapper)
    {
    }
}

public class BaseTrackingMapper<TSource, TDestination, TKey> :
    BaseTrackingMapperUnidirectional<TSource, TDestination, TKey>, IMapper<TSource, TDestination>
    where TSource : class
    where TDestination : class
    where TKey : struct, IEquatable<TKey>
{
    public BaseTrackingMapper(ITrackingAutoMapperWrapper mapper) : base(mapper)
    {
    }

    public TSource? Map(TDestination? entity)
    {
        return Mapper.Map<TDestination, TSource>(entity);
    }

    public TSource Map(TDestination source, TSource destination)
    {
        return Mapper.Map(source, destination);
    }
}