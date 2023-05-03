using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Domain;
using App.Domain.Enums;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class VideoService : BaseYouTubeService
{
    public VideoService(YouTubeUow youTubeUow) : base(youTubeUow)
    {
    }

    public async Task<VideoData> FetchVideoDataYtdl(string id, bool fetchComments, CancellationToken ct = default)
    {
        var videoResult = await YoutubeDl.RunVideoDataFetch(Url.ToVideoUrl(id), fetchComments: fetchComments, ct: ct);
        if (videoResult is not { Success: true })
        {
            throw new VideoNotFoundException(id);
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
        Context.QueueNewComments(videoData.ID);

        // TODO: Categories, Games
        Uow.Videos.Add(video);

        return video;
    }
}