using Microsoft.Extensions.Logging;

namespace Base.BLL;

public abstract class BaseLockedTimedBackgroundService<TService> : BaseTimedBackgroundService<TService>
{
    private readonly object _lock = new();
    private bool _isRunning;

    protected BaseLockedTimedBackgroundService(ILogger<TService> logger, IServiceProvider services, TimeSpan period) :
        base(logger, services, period)
    {
    }

    protected abstract Task DoLockedWork(object? state);
    protected virtual Task AfterLockedWork() => Task.CompletedTask;

    protected override async void DoWork(object? state)
    {
        lock (_lock)
        {
            if (_isRunning)
            {
                Logger.LogInformation($"Previous execution of background worker still running. Skipping execution.");
                return;
            }

            _isRunning = true;
        }

        try
        {
            await DoLockedWork(state);
        }
        finally
        {
            lock (_lock)
            {
                _isRunning = false;
            }
        }

        await AfterLockedWork();
    }
}