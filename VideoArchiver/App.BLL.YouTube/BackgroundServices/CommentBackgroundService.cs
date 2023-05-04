using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utils;
using YoutubeDLSharp.Metadata;

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
        await InitialAddUnfinishedCommentsAsync(ct);

        _onNewItemEnqueued = (_, videoId) => OnNewItemEnqueued(videoId, ct).Wait(ct);
        Context.NewCommentsQueued += _onNewItemEnqueued;
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
        if (ct.IsCancellationRequested) return;

        using var scope = Services.CreateScope();
        var youTubeUow = scope.GetYouTubeUow();
        var uow = youTubeUow.Uow;

        VideoData videoData;
        try
        {
            videoData = await youTubeUow.VideoService.FetchVideoDataYtdl(videoId, true, ct);
        }
        catch (VideoNotFoundException)
        {
            if (ct.IsCancellationRequested) return; // In case failure is caused by token cancellation?

            var failedCommentsVideo = await uow.Videos.GetByIdOnPlatformAsync(videoId, Platform.YouTube);
            if (failedCommentsVideo == null) return;

            // TODO: Status changes and notifications (Add StatusChange BG service to general BLL?)???
            failedCommentsVideo.PrivacyStatus = null;
            failedCommentsVideo.IsAvailable = false;

            await uow.SaveChangesAsync();

            return;
        }

        if (ct.IsCancellationRequested) return;

        Video? video = null;
        for (var i = 0; i < 3; i++)
        {
            video = await uow.Videos.GetByIdOnPlatformAsync(videoId, Platform.YouTube);
            if (video == null) ct.WaitHandle.WaitOne(10000);
        }

        if (video == null)
        {
            var e = new VideoNotFoundException(videoId);
            Logger.LogError(e, "Video with id {VideoId} not found in DB", videoId);
            return;
        }

        await youTubeUow.CommentService.AddComments(video, videoData.Comments, ct);

        await uow.SaveChangesAsync();
    }
}