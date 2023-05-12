namespace Contracts.DAL;

public interface IMapperUnidirectional<TSource, TDestination>
{
    TDestination? Map(TSource? entity);
    TDestination Map(TSource source, TDestination destination);
}

public interface IMapper<TSource, TDestination> : IMapperUnidirectional<TSource, TDestination>
{
    TSource? Map(TDestination? entity);
    TSource Map(TDestination source, TSource destination);
}