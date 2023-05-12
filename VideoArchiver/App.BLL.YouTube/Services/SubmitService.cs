using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.DTO;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.YouTube.Services;

public class SubmitService : BaseYouTubeService<SubmitService>, IPlatformUrlSubmissionHandler
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
                        await Uow.Playlists.GetByIdOnPlatformAsync(playlistId!, Platform.YouTube);
                    if (previouslyArchivedPlaylist == null)
                    {
                        result.ContainsNonArchivedPlaylist = true;
                    }
                    else
                    {
                        result.Add(PlaylistMapper.Map(previouslyArchivedPlaylist)!);
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

    private async Task<UrlSubmissionResult> SubmitVideo(string id, Guid submitterId, bool autoSubmit)
    {
        var previouslyArchivedVideo = await Uow.Videos.GetByIdOnPlatformAsync(id, Platform.YouTube);
        if (previouslyArchivedVideo != null)
        {
            Uow.QueueItems.Add(new App.DAL.DTO.Entities.QueueItem(submitterId, autoSubmit, previouslyArchivedVideo));
            UrlSubmissionResult result = VideoMapper.Map(previouslyArchivedVideo)!;
            result.AlreadyAdded = true;
            return result;
        }

        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(id, false);

        if (!autoSubmit)
        {
            return QueueItemMapper.Map(
                Uow.QueueItems.Add(new App.DAL.DTO.Entities.QueueItem(id, submitterId, autoSubmit, Platform.YouTube)))!;
        }

        var video = await YouTubeUow.VideoService.AddVideo(videoData);
        Uow.QueueItems.Add(new App.DAL.DTO.Entities.QueueItem(submitterId, autoSubmit, video));

        return VideoMapper.Map(video)!;
    }

    private async Task<UrlSubmissionResult> SubmitPlaylist(string id, Guid submitterId, bool autoSubmit)
    {
        var previouslyArchivedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(id, Platform.YouTube);
        if (previouslyArchivedPlaylist != null)
        {
            Uow.QueueItems.Add(new App.DAL.DTO.Entities.QueueItem(submitterId, autoSubmit, previouslyArchivedPlaylist));
            UrlSubmissionResult result = PlaylistMapper.Map(previouslyArchivedPlaylist)!;
            result.AlreadyAdded = true;
            return result;
        }

        var playlistData = await YouTubeUow.PlaylistService.FetchPlaylistDataYtdl(id);

        if (!autoSubmit)
        {
            return QueueItemMapper.Map(
                Uow.QueueItems.Add(new App.DAL.DTO.Entities.QueueItem(id, submitterId, autoSubmit, Platform.YouTube)))!;
        }

        var playlist = await YouTubeUow.PlaylistService.AddPlaylist(playlistData);
        Uow.QueueItems.Add(new App.DAL.DTO.Entities.QueueItem(submitterId, autoSubmit, playlist));

        return PlaylistMapper.Map(playlist)!;
    }
}