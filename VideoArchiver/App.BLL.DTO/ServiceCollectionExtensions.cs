using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.DTO;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllMappers(this IServiceCollection services)
    {
        return services;
    }
}