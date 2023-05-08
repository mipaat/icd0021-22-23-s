using System.Xml;
using App.BLL.YouTube.Utils;
using App.Domain.Enums;
using App.Domain.NotMapped;
using Google.Apis.YouTube.v3.Data;

namespace App.BLL.YouTube.Extensions;

public static class DomainExtensionsOfficial
{
    public static Domain.Video ToDomainVideo(this Video video,
        ImageFileList? previousThumbnails = null)
    {
        var title = new LangString(video.Snippet.Title,
            video.Snippet.DefaultLanguage ?? LangString.UnknownCulture);
        var description = new LangString(video.Snippet.Description,
            video.Snippet.DefaultLanguage ?? LangString.UnknownCulture);
        if (video.Localizations != null)
        {
            foreach (var kvp in video.Localizations)
            {
                title[kvp.Key] = kvp.Value.Title;
                description[kvp.Key] = kvp.Value.Description;
            }
        }

        return new Domain.Video
        {
            Platform = Platform.YouTube,
            IdOnPlatform = video.Id,

            Title = title,
            Description = description,

            DefaultLanguage = video.Snippet.DefaultLanguage,
            DefaultAudioLanguage = video.Snippet.DefaultAudioLanguage,

            Duration = XmlConvert.ToTimeSpan(video.ContentDetails.Duration),

            ViewCount = Convert.ToInt32(video.Statistics.ViewCount),
            LikeCount = Convert.ToInt32(video.Statistics.LikeCount),
            DislikeCount = Convert.ToInt32(video.Statistics.DislikeCount),
            CommentCount = Convert.ToInt32(video.Statistics.CommentCount),

            Tags = video.Snippet.Tags.ToList(),
            Thumbnails = previousThumbnails?.GetSnapShot(),

            IsLivestreamRecording = video.LiveStreamingDetails != null,
            LivestreamStartedAt = video.LiveStreamingDetails?.ActualStartTime?.ToUniversalTime(),
            LivestreamEndedAt = video.LiveStreamingDetails?.ActualEndTime?.ToUniversalTime(),

            PublishedAt = video.Snippet.PublishedAt?.ToUniversalTime(),
            RecordedAt = video.RecordingDetails?.RecordingDate?.ToUniversalTime(),
            CreatedAt = video.Snippet.PublishedAt?.ToUniversalTime(),
            UpdatedAt = DateTime.UtcNow,

            PrivacyStatus = video.Status?.PrivacyStatus?.ToDomainPrivacyStatus(),
            IsAvailable = video.Status?.PrivacyStatus?.ToDomainPrivacyStatus().IsAvailable() ?? true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            Etag = video.ETag,
            LastFetchOfficial = DateTime.UtcNow,
            LastSuccessfulFetchOfficial = DateTime.UtcNow,

            AddedToArchiveAt = DateTime.UtcNow,
        };
    }

    private static EPrivacyStatus? ToDomainPrivacyStatus(this string youTubeApiPrivacyStatus)
    {
        return youTubeApiPrivacyStatus switch
        {
            "public" => EPrivacyStatus.Public,
            "unlisted" => EPrivacyStatus.Unlisted,
            "private" => EPrivacyStatus.Unlisted,
            _ => null,
        };
    }

    public static Domain.Playlist ToDomainPlaylist(this Playlist playlist,
        ImageFileList? previousThumbnails = null)
    {
        var title = new LangString(playlist.Snippet.Title,
            playlist.Snippet.DefaultLanguage ?? LangString.UnknownCulture);
        var description = new LangString(playlist.Snippet.Description,
            playlist.Snippet.DefaultLanguage ?? LangString.UnknownCulture);
        if (playlist.Localizations != null)
        {
            foreach (var kvp in playlist.Localizations)
            {
                title[kvp.Key] = kvp.Value.Title;
                description[kvp.Key] = kvp.Value.Description;
            }
        }

        return new Domain.Playlist
        {
            Platform = Platform.YouTube,
            IdOnPlatform = playlist.Id,

            Title = title,
            Description = description,

            DefaultLanguage = playlist.Snippet.DefaultLanguage,

            Thumbnails = playlist.Snippet.Thumbnails?.ToDomainImageFiles(previousThumbnails),
        };
    }

    private static List<ImageFile> Thumbnails(this ThumbnailDetails thumbnailDetails)
    {
        var thumbnails = new List<ImageFile>();
        if (thumbnailDetails.Default__ != null)
            thumbnails.Add(thumbnailDetails.Default__.ToImageFile(ThumbnailQuality.Default.Name));
        if (thumbnailDetails.Standard != null)
            thumbnails.Add(thumbnailDetails.Standard.ToImageFile(ThumbnailQuality.Standard.Name));
        if (thumbnailDetails.Medium != null)
            thumbnails.Add(thumbnailDetails.Medium.ToImageFile(ThumbnailQuality.Medium.Name));
        if (thumbnailDetails.High != null)
            thumbnails.Add(thumbnailDetails.High.ToImageFile(ThumbnailQuality.High.Name));
        if (thumbnailDetails.Maxres != null)
            thumbnails.Add(thumbnailDetails.Maxres.ToImageFile(ThumbnailQuality.MaxRes.Name));
        return thumbnails;
    }

    private static ImageFile ToImageFile(this Thumbnail thumbnail, string quality)
    {
        return new ImageFile
        {
            Platform = Platform.YouTube,

            Quality = quality,
            Url = thumbnail.Url,
            Etag = thumbnail.ETag,
            Width = Convert.ToInt32(thumbnail.Width),
            Height = Convert.ToInt32(thumbnail.Height),
        };
    }

    private static ImageFileList ToDomainImageFiles(this ThumbnailDetails thumbnailDetails,
        ImageFileList? previousImageFiles = null)
    {
        previousImageFiles = previousImageFiles?.GetSnapShot();
        var newImageFiles = new ImageFileList();
        foreach (var thumbnail in thumbnailDetails.Thumbnails())
        {
            var previousThumbnail = previousImageFiles?.SingleOrDefault(i =>
                i.Url == thumbnail.Url && global::Utils.Utils.EqualsOrNull(i.Etag, thumbnail.Etag));
            if (previousThumbnail != null)
            {
                thumbnail.Key = previousThumbnail.Key;
                thumbnail.LocalFilePath = previousThumbnail.Key;
                thumbnail.Hash = previousThumbnail.Hash;
                newImageFiles.Add(thumbnail);
            }
        }

        return newImageFiles;
    }
}