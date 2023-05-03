using App.BLL.YouTube.BackgroundServices;
using App.BLL.YouTube.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.YouTube.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddYouTube(this IServiceCollection services)
    {
        services.AddSingleton<YouTubeContext>();

        services.AddScoped<YouTubeUow>();
        services.AddScoped<SubmitService>();

        services.AddHostedService<CommentBackgroundService>();
    }
}