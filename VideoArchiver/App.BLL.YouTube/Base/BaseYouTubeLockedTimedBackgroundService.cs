using Base.BLL;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.Base;

public abstract class BaseYouTubeLockedTimedBackgroundService<TService> : BaseLockedTimedBackgroundService<TService>
{
    protected readonly YouTubeContext YouTubeContext;

    public BaseYouTubeLockedTimedBackgroundService(ILogger<TService> logger, IServiceProvider services, TimeSpan period,
        YouTubeContext youTubeContext) : base(logger, services, period)
    {
        YouTubeContext = youTubeContext;
    }
}