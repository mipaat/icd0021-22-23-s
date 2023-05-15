using Base.BLL;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.Base;

public abstract class BaseYouTubeTimedBackgroundService<TService> : BaseTimedBackgroundService<TService>
{
    protected readonly YouTubeContext YouTubeContext;

    protected BaseYouTubeTimedBackgroundService(ILogger<TService> logger, IServiceProvider services, TimeSpan period,
        YouTubeContext youTubeContext) : base(logger, services, period)
    {
        YouTubeContext = youTubeContext;
    }
}