using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.BLL.YouTube.Utils;
using App.Common;
using App.DAL.DTO.Entities;
using App.Common.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class VideoService : BaseYouTubeService<VideoService>
{
    public VideoService(ServiceUow serviceUow, ILogger<VideoService> logger, YouTubeUow youTubeUow, IMapper mapper) :
        base(serviceUow,
            logger, youTubeUow, mapper)
    {
    }

    public async Task<VideoData> FetchVideoDataYtdl(string id, bool fetchComments, CancellationToken ct = default)
    {
        var videoResult = await YoutubeDl.RunVideoDataFetch(Url.ToVideoUrl(id), fetchComments: fetchComments, ct: ct);
        if (videoResult is not { Success: true })
        {
            ct.ThrowIfCancellationRequested();

            var failedVideo = await Uow.Videos.GetByIdOnPlatformAsync(id, EPlatform.YouTube);
            if (failedVideo != null)
            {
                await YouTubeUow.ServiceUow.StatusChangeService.Push(
                    new StatusChangeEvent(failedVideo, null, false));
            }

            throw new VideoNotFoundOnPlatformException(id, EPlatform.YouTube);
        }

        return videoResult.Data;
    }

    public async Task<Video> AddOrGetVideo(string id)
    {
        var video = await Uow.Videos.GetByIdOnPlatformAsync(id, EPlatform.YouTube);
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

    public async Task UpdateAddedVideoUnofficial(Video video)
    {
        // TODO: USE THIS
        video.LastFetchUnofficial = DateTime.UtcNow;
        VideoData videoData;
        try
        {
            videoData = await FetchVideoDataYtdl(video.IdOnPlatform, false);
        }
        catch (VideoNotFoundOnPlatformException)
        {
            await ServiceUow.StatusChangeService.Push(new StatusChangeEvent(video, null, false));
            return;
        }

        video.LastSuccessfulFetchUnofficial = video.LastFetchUnofficial;
        var domainVideo = videoData.ToDalVideo(video.Thumbnails);
        domainVideo.Thumbnails ??= ThumbnailUtils.GetAllPotentialThumbnails(domainVideo.IdOnPlatform);
        await ServiceUow.ImageService.UpdateThumbnails(domainVideo);
        await ServiceUow.EntityUpdateService.UpdateVideo(video, domainVideo);
    }

    public async Task<bool> UpdateAddedNeverFetchedVideosOfficial()
    {
        lock (Context.VideoUpdateLock)
        {
            if (Context.VideoUpdateOngoing) return false;
            Context.VideoUpdateOngoing = true;
        }

        var videos = await Uow.Videos.GetAllNotOfficiallyFetched(EPlatform.YouTube);
        var result = await UpdateAddedVideosOfficial(videos);

        lock (Context.VideoUpdateLock)
        {
            Context.VideoUpdateOngoing = false;
        }

        return result;
    }

    public async Task<bool> UpdateAddedVideosOfficial()
    {
        lock (Context.VideoUpdateLock)
        {
            if (Context.VideoUpdateOngoing) return false;
            Context.VideoUpdateOngoing = true;
        }

        var videos =
            await Uow.Videos.GetAllBeforeOfficialApiFetch(EPlatform.YouTube,
                DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)), 50);
        var result = await UpdateAddedVideosOfficial(videos);

        lock (Context.VideoUpdateLock)
        {
            Context.VideoUpdateOngoing = false;
        }

        return result;
    }

    private async Task<bool> UpdateAddedVideosOfficial(ICollection<Video> videos)
    {
        if (videos.Count == 0) return false;
        var fetchedVideos = await YouTubeUow.ApiService.FetchVideos(videos.Select(v => v.IdOnPlatform).ToList());
        foreach (var video in videos)
        {
            video.LastFetchOfficial = DateTime.UtcNow;
            var fetchedVideo = fetchedVideos.Items.SingleOrDefault(v => v.Id == video.IdOnPlatform);
            if (fetchedVideo == null) continue;
            video.LastSuccessfulFetchOfficial = video.LastFetchOfficial;
            var domainVideo = fetchedVideo.ToDalVideo(video.Thumbnails);
            domainVideo.Thumbnails ??= ThumbnailUtils.GetAllPotentialThumbnails(domainVideo.IdOnPlatform);
            await ServiceUow.ImageService.UpdateThumbnails(domainVideo);
            await ServiceUow.EntityUpdateService.UpdateVideo(video, domainVideo);
            Uow.Videos.Update(video);
        }

        return videos.Count == 50;
    }

    public async Task<Video> AddVideo(VideoData videoData)
    {
        var video = videoData.ToDalVideo();
        await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(video, videoData);

        /*
         * Add video ID to comments queue for background worker to fetch comments.
         * If application is stopped before comment fetching is finished,
         * comment fetching should resume by fetching video from DB with null comment fetch date.
         * TODO: Investigate if this causes potential scope/GC issues?
         */
        Uow.SavedChanges += (_, _) => Context.QueueNewComments(videoData.ID);
        Uow.SavedChanges += (_, _) => Context.QueueNewVideoDownload(videoData.ID);

        video.Thumbnails = ThumbnailUtils.GetAllPotentialThumbnails(video.IdOnPlatform);
        await ServiceUow.ImageService.UpdateThumbnails(video);

        // TODO: Categories, Games
        Uow.Videos.Add(video);

        return video;
    }

    public async Task DownloadVideo(string id, CancellationToken ct = default)
    {
        Logger.LogInformation("Started downloading video {IdOnPlatform} on platform {Platform}",
            id, EPlatform.YouTube);
        var result = await YoutubeDl.RunVideoDownload(Url.ToVideoUrl(id), ct: ct);
        if (result.Success)
        {
            var video = await YouTubeUow.VideoService.AddOrGetVideo(id);
            video.LocalVideoFiles = new List<VideoFile>
            {
                new()
                {
                    FilePath = global::Utils.Utils.MakeRelativeFilePath(result.Data),
                }
            };
            Uow.Videos.Update(video);
            Logger.LogInformation("Successfully downloaded video {IdOnPlatform} on platform {Platform}",
                id, EPlatform.YouTube);
        }
        else
        {
            Logger.LogError("Failed to download {Platform} video with ID {Id}.\nErrors: [{Errors}]", EPlatform.YouTube,
                id, result.ErrorOutput.Length > 0 ? string.Join("\n", result.ErrorOutput) : "Unknown errors");
            // TODO: Should download failure be tracked?
        }
    }
}