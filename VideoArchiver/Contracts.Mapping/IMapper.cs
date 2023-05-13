using AutoMapper;

namespace Contracts.Mapping;

public interface IMapperUnidirectional<TSource, TDestination>
{
    TDestination? Map(TSource? entity);
    TDestination? Map(TSource? entity, Action<IMappingOperationOptions<TSource, TDestination>>? opts);
    TDestination Map(TSource source, TDestination destination);

    TDestination Map(TSource source, TDestination destination,
        Action<IMappingOperationOptions<TSource, TDestination>>? opts);
}

public interface IMapper<TSource, TDestination> : IMapperUnidirectional<TSource, TDestination>
{
    TSource? Map(TDestination? entity);
    TSource? Map(TDestination? entity, Action<IMappingOperationOptions<TDestination, TSource>>? opts);
    TSource Map(TDestination source, TSource destination);

    TSource Map(TDestination source, TSource destination,
        Action<IMappingOperationOptions<TDestination, TSource>>? opts);
}