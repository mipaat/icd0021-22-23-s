namespace Contracts.Mapping;

public interface IMapperUnidirectional<in TSource, out TDestination>
{
    TDestination? Map(TSource? entity);
}

public interface IMapper<TSource, TDestination> : IMapperUnidirectional<TSource, TDestination>
{
    TSource? Map(TDestination? entity);
}