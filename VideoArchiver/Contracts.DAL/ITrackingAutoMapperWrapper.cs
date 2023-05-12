namespace Contracts.DAL;

public interface ITrackingAutoMapperWrapper
{
    public TDestination? Map<TSource, TDestination>(TSource? source)
        where TSource : class
        where TDestination : class;

    public TSource Map<TSource, TDestination>(TDestination source, TSource destination);
}