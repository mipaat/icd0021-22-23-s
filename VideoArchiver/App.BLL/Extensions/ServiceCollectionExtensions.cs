using App.BLL.BackgroundServices;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddGeneralBll(this IServiceCollection services)
    {
        services.AddSingleton<IServiceContext, ServiceContext>();

        services.AddScoped<IServiceUow, ServiceUow>();

        services.AddScoped<ISubmitService, SubmitService>();
        services.AddScoped<IVideoPresentationService, VideoPresentationService>();
        services.AddScoped<IEntityUpdateService, EntityUpdateService>();
        services.AddScoped<IEntityConcurrencyResolver, EntityConcurrencyResolver>();
        services.AddScoped<IStatusChangeService, StatusChangeService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IQueueItemService, QueueItemService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IVideoService, VideoService>();

        services.AddHostedService<QueueItemBackgroundService>();
    }
}