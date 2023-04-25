namespace Contracts.DAL;

public interface IMapper<TSource, TDestination>
{
    TDestination? Map(TSource? entity);
    TSource? Map(TDestination? entity);
}