namespace Contracts.DAL;

public interface IMapperUnidirectional<TSource, TDestination>
{
    TDestination? Map(TSource? entity);
}

public interface IMapper<TSource, TDestination> : IMapperUnidirectional<TSource, TDestination>
{
    TSource? Map(TDestination? entity);
}