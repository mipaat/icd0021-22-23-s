using AutoMapper;
using Contracts.Mapping;

namespace Base.Mapping;

public class BaseMapperUnidirectional<TSource, TDestination> : IMapperUnidirectional<TSource, TDestination>
{
    protected readonly IMapper Mapper;

    public BaseMapperUnidirectional(IMapper mapper)
    {
        Mapper = mapper;
    }

    public virtual TDestination? Map(TSource? entity)
    {
        return Map(entity, null);
    }

    public TDestination? Map(TSource? entity, Action<IMappingOperationOptions<TSource, TDestination>>? opts)
    {
        if (entity == null) return default;
        if (opts == null) return Mapper.Map<TSource, TDestination>(entity);
        return Mapper.Map(entity, opts);
    }

    public virtual TDestination Map(TSource source, TDestination destination)
    {
        return Map(source, destination, null);
    }

    public TDestination Map(TSource source, TDestination destination,
        Action<IMappingOperationOptions<TSource, TDestination>>? opts)
    {
        if (opts == null) return Mapper.Map(source, destination);
        return Mapper.Map(source, destination, opts);
    }
}

public class BaseMapper<TSource, TDestination> : BaseMapperUnidirectional<TSource, TDestination>,
    IMapper<TSource, TDestination>
{
    public BaseMapper(IMapper mapper) : base(mapper)
    {
    }

    public virtual TSource? Map(TDestination? entity)
    {
        return Map(entity, null);
    }

    public TSource? Map(TDestination? entity, Action<IMappingOperationOptions<TDestination, TSource>>? opts)
    {
        if (entity == null) return default;
        if (opts == null) return Mapper.Map<TSource>(entity);
        return Mapper.Map(entity, opts);
    }

    public TSource Map(TDestination source, TSource destination)
    {
        return Map(source, destination, null);
    }

    public TSource Map(TDestination source, TSource destination,
        Action<IMappingOperationOptions<TDestination, TSource>>? opts)
    {
        if (opts == null) return Mapper.Map(source, destination);
        return Mapper.Map(source, destination, opts);
    }
}