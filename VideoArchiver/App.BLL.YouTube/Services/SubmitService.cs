using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Domain;
using App.Domain.Enums;
using App.DTO;

namespace App.BLL.YouTube.Services;

public class SubmitService : BaseYouTubeService, IPlatformUrlSubmissionHandler
{
    public SubmitService(YouTubeUow youTubeUow) : base(youTubeUow)
    {
    }

    public bool IsPlatformUrl(string url) => Url.IsYouTubeUrl(url);

    public async Task<UrlSubmissionResults> SubmitUrl(string url, Guid submitterId, bool autoSubmit,
        bool alsoSubmitPlaylist)
    {
        // TODO: Adding video/playlist authors to video
        // TODO: Scheduled fetching from YouTube official API, accounting for rate limits
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
                        result.Add(previouslyArchivedPlaylist);
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
            Uow.QueueItems.Add(new QueueItem(submitterId, autoSubmit, previouslyArchivedVideo));
            UrlSubmissionResult result = previouslyArchivedVideo;
            result.AlreadyAdded = true;
            return result;
        }

        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(id, false);

        if (!autoSubmit)
        {
            return Uow.QueueItems.Add(new QueueItem(id, submitterId, autoSubmit, Platform.YouTube));
        }

        var video = await YouTubeUow.VideoService.AddVideo(videoData);
        Uow.QueueItems.Add(new QueueItem(submitterId, autoSubmit, video));

        return video;
    }

    private async Task<UrlSubmissionResult> SubmitPlaylist(string id, Guid submitterId, bool autoSubmit)
    {
        var previouslyArchivedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(id, Platform.YouTube);
        if (previouslyArchivedPlaylist != null)
        {
            Uow.QueueItems.Add(new QueueItem(submitterId, autoSubmit, previouslyArchivedPlaylist));
            UrlSubmissionResult result = previouslyArchivedPlaylist;
            result.AlreadyAdded = true;
            return result;
        }

        var playlistResult = await YoutubeDl.RunVideoDataFetch(Url.ToPlaylistUrl(id));
        if (playlistResult is not { Success: true })
        {
            throw new PlaylistNotFoundException(id);
        }

        if (!autoSubmit)
        {
            return Uow.QueueItems.Add(new QueueItem(id, submitterId, autoSubmit, Platform.YouTube));
        }

        var playlist = playlistResult.Data.ToDomainPlaylist();
        await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(playlist, playlistResult.Data);
        await YouTubeUow.PlaylistService.AddAndSetVideos(playlist, playlistResult.Data);
        Uow.QueueItems.Add(new QueueItem(submitterId, autoSubmit, playlist));
        Uow.Playlists.Add(playlist);

        return playlist;
    }
}