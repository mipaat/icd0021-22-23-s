using App.BLL.YouTube.BackgroundServices;
using App.BLL.YouTube.Services;
using App.BLL.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.YouTube.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddYouTube(this IServiceCollection services)
    {
        services.AddSingleton<YouTubeContext>();

        services.AddScoped<YouTubeUow>();
        services.AddScoped<IPlatformUrlSubmissionHandler, SubmitService>();

        services.AddScoped<SubmitService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<PlaylistService>();
        services.AddScoped<CommentService>();
        services.AddScoped<VideoService>();
        services.AddScoped<ApiService>();

        services.AddHostedService<CommentBackgroundService>();
        services.AddHostedService<OfficialApiTimedBackgroundService>();
    }
}