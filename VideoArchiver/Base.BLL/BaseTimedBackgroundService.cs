using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Base.BLL;

public abstract class BaseTimedBackgroundService<TService> : IHostedService, IDisposable
{
    protected readonly ILogger<TService> Logger;
    protected readonly IServiceProvider Services;
    protected Timer? Timer;
    private readonly TimeSpan _period;

    protected BaseTimedBackgroundService(ILogger<TService> logger, IServiceProvider services, TimeSpan period)
    {
        Logger = logger;
        Services = services;
        _period = period;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Starting background service");

        Timer = new Timer(DoWork, null, TimeSpan.Zero, _period);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Stopping background service");

        Timer?.Change(Timeout.Infinite, 0);
        
        return Task.CompletedTask;
    }

    protected abstract void DoWork(object? _);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Timer?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}