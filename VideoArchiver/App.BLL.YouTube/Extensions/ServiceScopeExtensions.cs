using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.YouTube.Extensions;

public static class ServiceScopeExtensions
{
    public static YouTubeUow GetYouTubeUow(this IServiceScope scope)
    {
        return scope.ServiceProvider.GetRequiredService<YouTubeUow>();
    }
}