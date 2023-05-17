using App.BLL.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.Base;

public abstract class BaseYouTubeBackgroundService<TBackgroundService> : BaseBackgroundService<TBackgroundService>
{
    protected readonly YouTubeContext YouTubeContext;

    protected BaseYouTubeBackgroundService(IServiceProvider services, ILogger<TBackgroundService> logger) :
        base(services, logger)
    {
        YouTubeContext = services.GetRequiredService<YouTubeContext>();
    }
}