using App.BLL.BackgroundServices;
using App.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddGeneralBll(this IServiceCollection services)
    {
        services.AddSingleton<ServiceContext>();

        services.AddScoped<ServiceUow>();

        services.AddScoped<SubmitService>();
        services.AddScoped<VideoPresentationHandler>();
        services.AddScoped<EntityUpdateService>();
        services.AddScoped<EntityConcurrencyResolver>();
        services.AddScoped<StatusChangeService>();
        services.AddScoped<ImageService>();
        services.AddScoped<AuthorizationService>();
        services.AddScoped<QueueItemService>();
        services.AddScoped<BasicGameCrudService>();

        services.AddHostedService<QueueItemBackgroundService>();
    }
}