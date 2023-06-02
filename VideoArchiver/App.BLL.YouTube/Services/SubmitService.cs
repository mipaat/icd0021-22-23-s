using App.BLL.Contracts;
using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.DTO.Entities;
using App.Common.Enums;
using App.DAL.DTO.Entities.Playlists;
using Microsoft.Extensions.Logging;
using Author = App.DAL.DTO.Entities.Author;
using Video = App.DAL.DTO.Entities.Video;

namespace App.BLL.YouTube.Services;

public class SubmitService : BaseYouTubeService<SubmitService>, IPlatformSubmissionHandler
{
    public SubmitService(IServiceUow serviceUow, ILogger<SubmitService> logger, YouTubeUow youTubeUow) :
        base(serviceUow, logger, youTubeUow)
    {
    }

    public bool IsPlatformUrl(string url) => Url.IsYouTubeUrl(url);

    public async Task<UrlSubmissionResult> SubmitUrl(string url, Guid submitterId, bool autoSubmit,
        bool submitPlaylist)
    {
        var isVideoUrl = Url.IsVideoUrl(url, out var videoId);
        var isPlaylistUrl = Url.IsPlaylistUrl(url, out var playlistId);
        if (isVideoUrl && !(isPlaylistUrl && submitPlaylist))
        {
            return await SubmitVideo(videoId!, submitterId, autoSubmit);
        }

        if (isPlaylistUrl)
        {
            return await SubmitPlaylist(playlistId!, submitterId, autoSubmit);
        }

        // TODO: Authors

        throw new UnrecognizedUrlException(url);
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
            return new UrlSubmissionResult(
                await ServiceUow.QueueItemService.Add(previouslyArchivedVideo, submitterId, autoSubmit), true);
        }

        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(id, false);

        if (!autoSubmit)
        {
            return new UrlSubmissionResult(
                ServiceUow.QueueItemService.Add(id, EPlatform.YouTube, EEntityType.Video, submitterId, autoSubmit),
                false);
        }

        var video = await YouTubeUow.VideoService.AddVideo(videoData);
        return new UrlSubmissionResult(await ServiceUow.QueueItemService.Add(video, submitterId, autoSubmit), false);
    }

    private async Task<UrlSubmissionResult> SubmitPlaylist(string id, Guid submitterId, bool autoSubmit)
    {
        var previouslyArchivedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(id, EPlatform.YouTube);
        if (previouslyArchivedPlaylist != null)
        {
            return new UrlSubmissionResult(
                await ServiceUow.QueueItemService.Add(previouslyArchivedPlaylist, submitterId, autoSubmit), true);
        }

        var playlistData = await YouTubeUow.PlaylistService.FetchPlaylistDataYtdl(id);

        if (!autoSubmit)
        {
            return new UrlSubmissionResult(
                ServiceUow.QueueItemService.Add(id, EPlatform.YouTube, EEntityType.Playlist, submitterId, autoSubmit),
                false);
        }

        var playlist = await YouTubeUow.PlaylistService.AddPlaylist(playlistData);
        return new UrlSubmissionResult(await ServiceUow.QueueItemService.Add(playlist, submitterId, autoSubmit), false);
    }
}