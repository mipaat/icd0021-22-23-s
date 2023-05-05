using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Contracts.DAL;
using App.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utils;

namespace App.BLL.YouTube.BackgroundServices;

public class CommentBackgroundService : BaseYouTubeBackgroundService<CommentBackgroundService>
{
    public CommentBackgroundService(IServiceProvider services, ILogger<CommentBackgroundService> logger) : base(
        services, logger)
    {
        _taskQueue = new BackgroundTaskQueue<Task>(1); // TODO: Make capacity configurable? Or automatic somehow?
    }

    private readonly BackgroundTaskQueue<Task> _taskQueue;

    private EventHandler<string>? _onNewItemEnqueued;

    private async Task InitialAddUnfinishedCommentsAsync(CancellationToken ct)
    {
        ICollection<string> videoIds;
        using (var scope = Services.CreateScope())
        {
            videoIds = await scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>().Videos
                .GetAllIdsOnPlatformWithUnarchivedComments(Platform.YouTube);
        }

        foreach (var videoId in videoIds)
        {
            if (ct.IsCancellationRequested) break;
            await OnNewItemEnqueued(videoId, ct);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _onNewItemEnqueued = (_, videoId) => OnNewItemEnqueued(videoId, ct).Wait(ct);
        Context.NewCommentsQueued += _onNewItemEnqueued;

        await InitialAddUnfinishedCommentsAsync(ct);

        while (!ct.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(ct);
            await workItem(ct);
        }

        Unsubscribe();
    }

    private void Unsubscribe()
    {
        if (_onNewItemEnqueued == null) return;
        Context.NewCommentsQueued -= _onNewItemEnqueued;
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

    private async Task OnNewItemEnqueued(string videoId, CancellationToken ct)
    {
        Logger.LogInformation("Started updating comments for video {IdOnPlatform} on platform {Platform}",
            videoId, Platform.YouTube);

        using var scope = Services.CreateScope();
        var youTubeUow = scope.GetYouTubeUow();
        var uow = youTubeUow.Uow;

        try
        {
            await youTubeUow.CommentService.UpdateComments(videoId, ct);
        }
        catch (VideoNotFoundOnPlatformException e)
        {
            Logger.LogError(exception: e, "Video {IdOnPlatform} not found on platform {Platform}",
                videoId, Platform.YouTube);
        }
        catch (OperationCanceledException)
        {
            Logger.LogWarning("Cancelled updating comments for video {IdOnPlatform} on platform {Platform}",
                videoId, Platform.YouTube);
        }

        await uow.SaveChangesAsync();
    }
}