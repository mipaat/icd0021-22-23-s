using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App.BLL.Base;

public abstract class BaseBackgroundService<TBackgroundService> : BackgroundService
{
    protected readonly IServiceProvider Services;
    protected readonly ILogger<TBackgroundService> Logger;
    protected readonly ServiceContext ServiceContext;

    protected BaseBackgroundService(IServiceProvider services, ILogger<TBackgroundService> logger)
    {
        Services = services;
        Logger = logger;
        ServiceContext = services.GetRequiredService<ServiceContext>();
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