using System.Xml;
using App.BLL.YouTube.Utils;
using App.Common.Enums;
using App.Common;
using Google.Apis.YouTube.v3.Data;

namespace App.BLL.YouTube.Extensions;

public static class ConversionExtensionsOfficial
{
    public static App.DAL.DTO.Entities.Video ToDalVideo(this Video video,
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

        return new App.DAL.DTO.Entities.Video
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = video.Id,

            Title = title,
            Description = description,

            DefaultLanguage = video.Snippet.DefaultLanguage,
            DefaultAudioLanguage = video.Snippet.DefaultAudioLanguage,

            Duration = video.ContentDetails.Duration != null
                ? XmlConvert.ToTimeSpan(video.ContentDetails.Duration)
                : null,

            ViewCount = ToInt32(video.Statistics.ViewCount),
            LikeCount = ToInt32(video.Statistics.LikeCount),
            DislikeCount = ToInt32(video.Statistics.DislikeCount),
            CommentCount = ToInt32(video.Statistics.CommentCount),

            Tags = video.Snippet.Tags?.ToList(),
            Thumbnails = previousThumbnails?.GetSnapShot(),

            IsLivestreamRecording = video.LiveStreamingDetails != null,
            LivestreamStartedAt = video.LiveStreamingDetails?.ActualStartTime?.ToUniversalTime(),
            LivestreamEndedAt = video.LiveStreamingDetails?.ActualEndTime?.ToUniversalTime(),

            PublishedAt = video.Snippet.PublishedAt?.ToUniversalTime(),
            RecordedAt = video.RecordingDetails?.RecordingDate?.ToUniversalTime(),
            CreatedAt = video.Snippet.PublishedAt?.ToUniversalTime(),
            UpdatedAt = DateTime.UtcNow,

            PrivacyStatus = video.Status?.PrivacyStatus?.ToDalPrivacyStatus(),
            IsAvailable = video.Status?.PrivacyStatus?.ToDalPrivacyStatus().IsAvailable() ?? true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            Etag = video.ETag,
            LastFetchOfficial = DateTime.UtcNow,
            LastSuccessfulFetchOfficial = DateTime.UtcNow,

            AddedToArchiveAt = DateTime.UtcNow,
        };
    }

    private static EPrivacyStatus? ToDalPrivacyStatus(this string youTubeApiPrivacyStatus)
    {
        return youTubeApiPrivacyStatus switch
        {
            "public" => EPrivacyStatus.Public,
            "unlisted" => EPrivacyStatus.Unlisted,
            "private" => EPrivacyStatus.Unlisted,
            _ => null,
        };
    }

    public static DAL.DTO.Entities.Playlists.Playlist ToDalPlaylist(this Playlist playlist,
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

        return new DAL.DTO.Entities.Playlists.Playlist
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = playlist.Id,

            Title = title,
            Description = description,

            DefaultLanguage = playlist.Snippet.DefaultLanguage,

            Thumbnails = playlist.Snippet.Thumbnails?.ToDalImageFiles(previousThumbnails),
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
            Platform = EPlatform.YouTube,

            Quality = quality,
            Url = thumbnail.Url,
            Etag = thumbnail.ETag,
            Width = ToInt32(thumbnail.Width),
            Height = ToInt32(thumbnail.Height),
        };
    }

    private static ImageFileList ToDalImageFiles(this ThumbnailDetails thumbnailDetails,
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

    private static int? ToInt32(object? value)
    {
        if (value == null) return null;
        return Convert.ToInt32(value);
    }
}