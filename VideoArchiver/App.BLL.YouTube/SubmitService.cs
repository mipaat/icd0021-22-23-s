using App.BLL.Exceptions;
using App.Domain;
using App.Domain.Enums;
using App.DTO;
using YoutubeDLSharp;

namespace App.BLL.YouTube;

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
                    // result.Add(await SubmitPlaylist(playlistId!, submitterId, autoSubmit));
                }
                else
                {
                    var previouslyArchivedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(playlistId!, Platform.YouTube);
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
                // result.Add(await SubmitPlaylist(playlistId!, submitterId, autoSubmit));
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

        var videoResult = await YoutubeDl.RunVideoDataFetch(Url.ToVideoUrl(id), fetchComments: true);
        if (videoResult is not { Success: true })
        {
            throw new VideoNotFoundException(id);
        }

        if (!autoSubmit)
        {
            return new QueueItem(id, submitterId, autoSubmit, Platform.YouTube);
        }

        var video = videoResult.Data.ToDomainVideo();
        await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(video, videoResult.Data);
        await YouTubeUow.CommentService.AddComments(video, videoResult.Data.Comments);
        // TODO: Comments, Categories, Games
        Uow.QueueItems.Add(new QueueItem(submitterId, autoSubmit, video));
        Uow.Videos.Add(video);
        if (video.Comments != null)
        {
            Uow.Comments.AddRange(video.Comments);            
        }

        return video;
    }

    // private async Task<UrlSubmissionResult> SubmitPlaylist(string id, Guid submitterId, bool autoSubmit)
    // {
    //     var previouslyArchivedPlaylist = await Uow.Playlists.GetByIdOnPlatformAsync(id, Platform.YouTube);
    //
    //     var queueItem = previouslyArchivedPlaylist != null
    //         ? new QueueItem(submitterId, autoSubmit, previouslyArchivedPlaylist)
    //         : new QueueItem(id, submitterId, autoSubmit, Platform.YouTube);
    //     Uow.QueueItems.Add(queueItem);
    //
    //     if (previouslyArchivedPlaylist != null)
    //     {
    //         UrlSubmissionResult result = previouslyArchivedPlaylist;
    //         result.AlreadyAdded = true;
    //         return result;
    //     }
    //
    //     var playlist = await YouTubeExplodeClient.Playlists.GetAsync(id);
    //     if (playlist == null)
    //     {
    //         throw new PlaylistNotFoundException(id);
    //     }
    //
    //     if (!autoSubmit)
    //     {
    //         return queueItem;
    //     }
    //
    //     var domainPlaylist = playlist.ToDomainPlaylist();
    //     Uow.Playlists.Add(domainPlaylist);
    //
    //     if (playlist.Author != null)
    //     {
    //         await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(domainPlaylist, playlist.Author);
    //     }
    //
    //     return domainPlaylist;
    // }
}