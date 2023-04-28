using System.Configuration;
using App.BLL.Exceptions;
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Enums;
using App.DTO;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using YoutubeExplode;

namespace App.BLL.YouTube;

public class SubmitService : IPlatformUrlSubmissionHandler
{
    private readonly IAppUnitOfWork _uow;
    private readonly YoutubeClient _youTubeExplodeClient;
    private readonly YouTubeSettings _config;

    public SubmitService(IAppUnitOfWork uow, IConfiguration configuration)
    {
        _config = configuration.GetRequiredSection(YouTubeSettings.SectionKey).Get<YouTubeSettings>() ?? throw new ConfigurationErrorsException("Failed to read YouTube configuration!");
        _uow = uow;
        _youTubeExplodeClient = new YoutubeClient();
    }

    public bool IsPlatformUrl(string url) => Url.IsYouTubeUrl(url);

    public async Task<EntityOrQueueItem> SubmitUrl(string url, Guid submitterId, bool autoSubmit)
    {
        // TODO: Adding video/playlist authors to video
        // TODO: Scheduled fetching from YouTube official API, accounting for rate limits
        // TODO: Scheduled? downloads? Comments?

        // TODO: What to do when link is a video & playlist link?

        if (Url.IsVideoUrl(url, out var id))
        {
            return await SubmitVideo(id!, submitterId, autoSubmit);
        }

        // TODO: Playlists, Authors
        // TODO: Content fetching VS metadata fetching?

        throw new UnrecognizedUrlException(url);
    }

    private async Task<EntityOrQueueItem> SubmitVideo(string id, Guid submitterId, bool autoSubmit)
    {
        var previouslyArchivedVideo = await _uow.Videos.GetByIdOnPlatformAsync(id);

        var queueItem = previouslyArchivedVideo != null
            ? new QueueItem(submitterId, autoSubmit, previouslyArchivedVideo)
            : new QueueItem(id, submitterId, autoSubmit, Platform.YouTube);
        _uow.QueueItems.Add(queueItem);

        if (previouslyArchivedVideo != null)
        {
            throw new EntityAlreadyAddedException(previouslyArchivedVideo);
        }

        var video = await _youTubeExplodeClient.Videos.GetAsync(id);
        if (video == null)
        {
            throw new VideoNotFoundException(id);
        }

        if (!autoSubmit)
        {
            return queueItem;
        }

        var domainVideo = video.ToDomainVideo();
        _uow.Videos.Add(domainVideo);
        return domainVideo;
    }
}