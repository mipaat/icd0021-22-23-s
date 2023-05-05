using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Domain;
using App.Domain.Enums;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class VideoService : BaseYouTubeService<VideoService>
{
    public VideoService(YouTubeUow youTubeUow, ILogger<VideoService> logger) : base(youTubeUow, logger)
    {
    }

    public async Task<VideoData> FetchVideoDataYtdl(string id, bool fetchComments, CancellationToken ct = default)
    {
        var videoResult = await YoutubeDl.RunVideoDataFetch(Url.ToVideoUrl(id), fetchComments: fetchComments, ct: ct);
        if (videoResult is not { Success: true })
        {
            ct.ThrowIfCancellationRequested();

            var failedVideo = await Uow.Videos.GetByIdOnPlatformAsync(id, Platform.YouTube);
            if (failedVideo != null)
            {
                // TODO: Status changes and notifications (Add StatusChange BG service to general BLL?)???
                failedVideo.PrivacyStatus = null;
                failedVideo.IsAvailable = false;
            }

            throw new VideoNotFoundOnPlatformException(id, Platform.YouTube);
        }

        return videoResult.Data;
    }

    public async Task<Video> AddOrGetVideo(string id)
    {
        var video = await Uow.Videos.GetByIdOnPlatformAsync(id, Platform.YouTube);
        if (video != null)
        {
            return video;
        }

        return await AddVideo(id);
    }

    public async Task<Video> AddVideo(string id)
    {
        return await AddVideo(await FetchVideoDataYtdl(id, false));
    }

    public async Task<Video> AddVideo(VideoData videoData)
    {
        var video = videoData.ToDomainVideo();
        await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(video, videoData);

        /*
         * Add video ID to comments queue for background worker to fetch comments.
         * If application is stopped before comment fetching is finished,
         * comment fetching should resume by fetching video from DB with null comment fetch date. 
         */
        Uow.SavedChanges += (_, _) => Context.QueueNewComments(videoData.ID);

        // TODO: Categories, Games
        Uow.Videos.Add(video);

        return video;
    }
}