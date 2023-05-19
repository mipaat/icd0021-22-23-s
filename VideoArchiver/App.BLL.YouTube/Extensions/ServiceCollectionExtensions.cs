using App.BLL.YouTube.BackgroundServices;
using App.BLL.YouTube.Services;
using App.BLL.DTO.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.YouTube.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddYouTube(this IServiceCollection services)
    {
        services.AddSingleton<YouTubeContext>();

        services.AddScoped<YouTubeUow>();
        services.AddScoped<IPlatformSubmissionHandler, SubmitService>();

        services.AddScoped<SubmitService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<PlaylistService>();
        services.AddScoped<CommentService>();
        services.AddScoped<VideoService>();
        services.AddScoped<ApiService>();

        services.AddScoped<IPlatformVideoPresentationHandler, PresentationHandler>();

        services.AddHostedService<CommentBackgroundService>();
        services.AddHostedService<OfficialApiTimedBackgroundService>();
        services.AddHostedService<PlaylistTimedBackgroundService>();
        services.AddHostedService<VideoDownloadBackgroundService>();
    }
}