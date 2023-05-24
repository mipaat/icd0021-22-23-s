using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Common.Enums;
using App.Contracts.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utils;

namespace App.BLL.YouTube.BackgroundServices;

public class VideoDownloadBackgroundService : BaseYouTubeBackgroundService<VideoDownloadBackgroundService>
{
    public VideoDownloadBackgroundService(IServiceProvider services, ILogger<VideoDownloadBackgroundService> logger) :
        base(
            services, logger)
    {
        _taskQueue = new BackgroundTaskQueue<Task>(4);
    }

    private readonly BackgroundTaskQueue<Task> _taskQueue;

    private EventHandler<string>? _onNewItemEnqueued;

    private async Task InitialAddUnfinishedCommentsAsync(CancellationToken ct)
    {
        ICollection<string> videoIds;
        using (var scope = Services.CreateScope())
        {
            videoIds = await scope.ServiceProvider.GetRequiredService<IAppUnitOfWork>().Videos
                .GetAllIdsOnPlatformNotDownloaded(EPlatform.YouTube);
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
        YouTubeContext.NewVideoQueuedForDownload += _onNewItemEnqueued;

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
        YouTubeContext.NewCommentsQueued -= _onNewItemEnqueued;
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
        using var scope = Services.CreateScope();
        var youTubeUow = scope.GetYouTubeUow();
        var uow = youTubeUow.Uow;

        try
        {
            await youTubeUow.VideoService.DownloadVideo(videoId, ct);
            await uow.SaveChangesAsync();
        }
        catch (VideoNotFoundOnPlatformException e)
        {
            Logger.LogError(exception: e, "Video {IdOnPlatform} not found on platform {Platform}",
                videoId, EPlatform.YouTube);
        }
        catch (VideoNotFoundInArchiveException e)
        {
            Logger.LogError(exception: e, "Video {IdOnPlatform} not found on platform {Platform}",
                videoId, EPlatform.YouTube);
        }
        catch (OperationCanceledException)
        {
            Logger.LogWarning("Cancelled downloading video {IdOnPlatform} on platform {Platform}",
                videoId, EPlatform.YouTube);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "Exception occurred when downloading video {IdOnPlatform} on platform {Platform}.",
                videoId, EPlatform.YouTube);
        }
    }
}