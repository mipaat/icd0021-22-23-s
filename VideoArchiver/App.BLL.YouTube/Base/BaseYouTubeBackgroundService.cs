using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utils;

namespace App.BLL.YouTube.Base;

public abstract class BaseYouTubeBackgroundService<TBackgroundService> : BackgroundService
{
    protected readonly ILogger<TBackgroundService> Logger;
    protected readonly IServiceProvider Services;
    protected readonly YouTubeContext Context;

    protected BaseYouTubeBackgroundService(IServiceProvider services, ILogger<TBackgroundService> logger)
    {
        Context = services.GetService<YouTubeContext>().RaiseIfNull();
        Services = services;
        Logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Starting");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Stopping");
        return base.StopAsync(cancellationToken);
    }
}