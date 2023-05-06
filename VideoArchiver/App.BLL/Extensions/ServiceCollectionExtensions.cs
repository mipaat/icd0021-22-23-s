using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddGeneralBll(this IServiceCollection services)
    {
        services.AddScoped<UrlSubmissionHandler>();
        services.AddScoped<EntityUpdateHandler>();
        services.AddScoped<EntityConcurrencyResolver>();
    }
}