using App.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddGeneralBll(this IServiceCollection services)
    {
        services.AddScoped<UrlSubmissionHandler>();
        services.AddScoped<EntityUpdateService>();
        services.AddScoped<EntityConcurrencyResolver>();
        services.AddScoped<StatusChangeService>();
        services.AddScoped<ImageService>();
        services.AddScoped<ServiceUow>();
    }
}