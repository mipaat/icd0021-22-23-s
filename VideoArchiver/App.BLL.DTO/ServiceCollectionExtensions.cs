using App.BLL.DTO.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.DTO;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllMappers(this IServiceCollection services)
    {
        services.AddScoped<GameMapper>();
        return services;
    }
}