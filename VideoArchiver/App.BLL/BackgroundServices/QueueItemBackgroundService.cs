using App.BLL.Base;
using App.BLL.Contracts;
using App.DAL.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utils;

namespace App.BLL.BackgroundServices;

public class QueueItemBackgroundService : BaseBackgroundService<QueueItemBackgroundService>
{
    public QueueItemBackgroundService(IServiceProvider services, ILogger<QueueItemBackgroundService> logger) : base(
        services, logger)
    {
        _taskQueue = new BackgroundTaskQueue<Task>(5);
    }

    private readonly BackgroundTaskQueue<Task> _taskQueue;
    private EventHandler<Guid>? _onNewItemEnqueued;

    private async Task InitialAddApprovedNotCompletedQueueItemsAsync(CancellationToken ct)
    {
        ICollection<Guid> queueItemIds;
        using (var scope = Services.CreateScope())
        {
            queueItemIds = await scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>().QueueItems
                .GetAllApprovedNotCompletedAsync();
        }

        foreach (var queueItemId in queueItemIds)
        {
            if (ct.IsCancellationRequested) break;
            await OnNewItemEnqueued(queueItemId);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _onNewItemEnqueued = (_, queueItemId) => OnNewItemEnqueued(queueItemId).Wait(ct);
        ServiceContext.NewQueueItemApproved += _onNewItemEnqueued;

        await InitialAddApprovedNotCompletedQueueItemsAsync(ct);

        while (!ct.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(ct);
            await workItem(ct);
        }

        Unsubscribe();
    }

    private async Task OnNewItemEnqueued(Guid queueItemId)
    {
        Logger.LogInformation("Started handling approved queue item {Id}", queueItemId);

        using var scope = Services.CreateScope();
        var serviceUow = scope.ServiceProvider.GetRequiredService<IServiceUow>();
        var uow = serviceUow.Uow;

        var queueItem = await uow.QueueItems.GetByIdAsync(queueItemId);
        if (queueItem == null)
        {
            Logger.LogError("Queue item {Id} not found", queueItemId);
            return;
        }

        try
        {
            await serviceUow.SubmitService.SubmitQueueItemAsync(queueItem);
        }
        catch (Exception e)
        {
            Logger.LogError(exception: e, "Error occurred while handling approved queue item {Id}", queueItemId);
            return;
        }

        try
        {
            await uow.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(exception: e,
                "Error occurred while attempting to save changes after handling approved queue item {Id}", queueItemId);
        }
    }

    private void Unsubscribe()
    {
        if (_onNewItemEnqueued == null) return;
        ServiceContext.NewQueueItemApproved -= _onNewItemEnqueued;
        _onNewItemEnqueued = null;
    }

    public override void Dispose()
    {
        base.Dispose();
        Unsubscribe();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        Unsubscribe();
    }
}