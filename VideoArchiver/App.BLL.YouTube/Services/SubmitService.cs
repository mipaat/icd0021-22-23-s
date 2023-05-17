using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.DTO.Contracts;
using App.BLL.DTO.Entities;
using App.Common.Enums;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Author = App.DAL.DTO.Entities.Author;
using Video = App.DAL.DTO.Entities.Video;

namespace App.BLL.YouTube.Services;

public class SubmitService : BaseYouTubeService<SubmitService>, IPlatformSubmissionHandler
{
    public SubmitService(ServiceUow serviceUow, ILogger<SubmitService> logger, YouTubeUow youTubeUow, IMapper mapper) :
        base(serviceUow, logger, youTubeUow, mapper)
    {
    }

    public bool IsPlatformUrl(string url) => Url.IsYouTubeUrl(url);

    public async Task<UrlSubmissionResults> SubmitUrl(string url, Guid submitterId, bool autoSubmit,
        bool alsoSubmitPlaylist)
    {
        // TODO: Scheduled fetching from YouTube official API (for localized titles/descriptions), accounting for rate limits
        // TODO: Scheduled downloads? Comments?

        // TODO: What to do when link is a video & playlist link?

        var result = new UrlSubmissionResults();

        var isVideoUrl = Url.IsVideoUrl(url, out var videoId);
        if (isVideoUrl)
        {
            result.Add(await SubmitVideo(videoId!, submitterId, autoSubmit));
        }

        if (Url.IsPlaylistUrl(url, out var playlistId))
        {
            if (isVideoUrl)
            {
                if (alsoSubmitPlaylist)
                {
                    result.Add(await SubmitPlaylist(playlistId!, submitterId, autoSubmit));
                }
                else
                {
                    var previouslyArchivedPlaylist =
                        await Uow.Playlists.GetByIdOnPlatformAsync(playlistId!, EPlatform.YouTube);
                    if (previouslyArchivedPlaylist == null)
                    {
                        result.ContainsNonArchivedPlaylist = true;
                    }
                    else
                    {
                        result.Add(new UrlSubmissionResult(previouslyArchivedPlaylist, true));
                    }
                }
            }
            else
            {
                result.Add(await SubmitPlaylist(playlistId!, submitterId, autoSubmit));
            }
        }

        // TODO: Authors
        // TODO: Content fetching VS metadata fetching?

        if (result.Count == 0) throw new UnrecognizedUrlException(url);

        return result;
    }

    public bool CanHandle(EPlatform platform, EEntityType entityType)
    {
        return platform == EPlatform.YouTube &&
               entityType is EEntityType.Video or EEntityType.Playlist; // TODO: Add author support
    }

    public async Task<Video> SubmitVideo(string idOnPlatform)
    {
        return await YouTubeUow.VideoService.AddOrGetVideo(idOnPlatform);
    }

    public async Task<Playlist> SubmitPlaylist(string idOnPlatform)
    {
        return await YouTubeUow.PlaylistService.AddOrGetPlaylist(idOnPlatform);
    }

    public Task<Author> SubmitAuthor(string idOnPlatform)
    {
        throw new NotImplementedException(); // TODO: Add author support
    }

    private async Task<UrlSubmissionResult> SubmitVideo(string id, Guid submitterId, bool autoSubmit)
    {
        var previouslyArchivedVideo = await Uow.Videos.GetByIdOnPlatformAsync(id, EPlatform.YouTube);
        if (previouslyArchivedVideo != null)
        {
            await ServiceUow.QueueItemService.Add(previouslyArchivedVideo, submitterId, autoSubmit);
            return new UrlSubmissionResult(previouslyArchivedVideo, true);
        }

        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(id, false);

        if (!autoSubmit)
        {
            var queueItem = ServiceUow.QueueItemService.Add(id, EPlatform.YouTube, EEntityType.Video, submitterId, autoSubmit);
            return new UrlSubmissionResult(queueItem);
        }

        var video = await YouTubeUow.VideoService.AddVideo(videoData);
        await ServiceUow.QueueItemService.Add(video, submitterId, autoSubmit);

        return new UrlSubmissionResult(video);
    }

    private async Task<UrlSubmissionResult> SubmitPlaylist(string id, Guid submitterId, bool autoSubmit)
    {
        var previouslyArchivedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(id, EPlatform.YouTube);
        if (previouslyArchivedPlaylist != null)
        {
            await ServiceUow.QueueItemService.Add(previouslyArchivedPlaylist, submitterId, autoSubmit);
            return new UrlSubmissionResult(previouslyArchivedPlaylist, true);
        }

        var playlistData = await YouTubeUow.PlaylistService.FetchPlaylistDataYtdl(id);

        if (!autoSubmit)
        {
            var queueItem = ServiceUow.QueueItemService.Add(id, EPlatform.YouTube, EEntityType.Playlist, submitterId, autoSubmit);
            return new UrlSubmissionResult(queueItem);
        }

        var playlist = await YouTubeUow.PlaylistService.AddPlaylist(playlistData);
        await ServiceUow.QueueItemService.Add(playlist, submitterId, autoSubmit);

        return new UrlSubmissionResult(playlist);
    }
}