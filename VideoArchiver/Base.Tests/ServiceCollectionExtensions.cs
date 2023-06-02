using Microsoft.Extensions.DependencyInjection;

namespace Base.Tests;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RemoveService<TService>(this IServiceCollection serviceCollection)
    {
        var descriptor = serviceCollection.SingleOrDefault(d => d.ServiceType == typeof(TService));
        if (descriptor != null)
        {
            serviceCollection.Remove(descriptor);
        }

        return serviceCollection;
    }
}