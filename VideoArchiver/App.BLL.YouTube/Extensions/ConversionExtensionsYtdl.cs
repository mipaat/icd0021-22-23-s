using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;
using App.Common;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Extensions;

public static class ConversionExtensionsYtdl
{
    public static Video ToDalVideo(this VideoData videoData, ImageFileList? previousThumbnails = null)
    {
        var domainVideo = new Video
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = videoData.ID,

            Title = new LangString(videoData.Title, LangString.UnknownCulture),
            Description = new LangString(videoData.Description, LangString.UnknownCulture),

            Duration = videoData.Duration != null ? TimeSpan.FromSeconds(videoData.Duration.Value) : null,

            ViewCount = videoData.ViewCount,
            LikeCount = videoData.LikeCount,
            DislikeCount = videoData.DislikeCount,
            CommentCount = videoData.CommentCount,

            Captions = videoData.Subtitles.ToDalCaptions(),
            AutomaticCaptions = videoData.AutomaticCaptions.ToDalCaptions(),
            Thumbnails = previousThumbnails?.GetSnapShot(),
            Tags = videoData.Tags.ToList(),

            IsLivestreamRecording = videoData.WasLive ?? videoData.IsLive,
            LivestreamStartedAt = (videoData.WasLive ?? videoData.IsLive) == true ? videoData.ReleaseTimestamp : null,

            CreatedAt = videoData.UploadDate,
            PublishedAt = videoData.ReleaseTimestamp,
            UpdatedAt = videoData.ModifiedTimestamp,

            PrivacyStatus = videoData.Availability.ToDalPrivacyStatus(),
            IsAvailable = videoData.Availability.ToDalPrivacyStatus().IsAvailable(),

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,
        };

        return domainVideo;
    }

    public static Author ToDalAuthor(this VideoData videoData, bool monitor = false, bool download = false)
    {
        var domainAuthor = new Author
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = videoData.ChannelID,

            UserName = Url.IsAuthorHandleUrl(videoData.ChannelUrl, out var handle) ? handle : null,
            DisplayName = videoData.Channel,

            SubscriberCount = videoData.ChannelFollowerCount,

            IsAvailable = true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,

            Monitor = monitor,
            Download = download,
        };

        return domainAuthor;
    }

    public static Author ToDalAuthor(this CommentData commentData)
    {
        var domainAuthor = new Author
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = commentData.AuthorID,

            DisplayName = commentData.Author,

            IsAvailable = true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            ProfileImages = new ImageFileList
            {
                new()
                {
                    Platform = EPlatform.YouTube,
                    Url = commentData.AuthorThumbnail,
                }
            },

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,

            Monitor = false,
            Download = false,
        };

        return domainAuthor;
    }

    public static Comment ToDalComment(this CommentData commentData)
    {
        return new Comment
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = commentData.ID,

            Content = commentData.Text,

            LikeCount = commentData.LikeCount,
            DislikeCount = commentData.DislikeCount,
            IsFavorited = commentData.IsFavorited,

            AuthorIsCreator = commentData.AuthorIsUploader,

            CreatedAt = commentData.Timestamp.ToUniversalTime(),

            IsAvailable = true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow
        };
    }

    public static CaptionsDictionary ToDalCaptions(
        this Dictionary<string, SubtitleData[]> youTubeDlCaptions)
    {
        var domainCaptions = new CaptionsDictionary();
        foreach (var key in youTubeDlCaptions.Keys)
        {
            domainCaptions[key] = youTubeDlCaptions[key]
                .Select(youTubeDlCaption => youTubeDlCaption.ToDalCaption()).ToList();
        }

        return domainCaptions;
    }

    public static EPrivacyStatus? ToDalPrivacyStatus(this Availability? availability)
    {
        return availability switch
        {
            Availability.Private => EPrivacyStatus.Private,
            Availability.PremiumOnly => EPrivacyStatus.PremiumOnly,
            Availability.SubscriberOnly => EPrivacyStatus.SubscriberOnly,
            Availability.NeedsAuth => EPrivacyStatus.NeedsAuth,
            Availability.Unlisted => EPrivacyStatus.Unlisted,
            Availability.Public => EPrivacyStatus.Public,
            null => null,
            _ => throw new ArgumentException($"Unknown {typeof(Availability)} Enum value '{availability}'",
                nameof(availability)),
        };
    }

    public static Caption ToDalCaption(this SubtitleData youTubeDlCaption)
    {
        var domainCaption = new Caption
        {
            Platform = EPlatform.YouTube,
            Ext = youTubeDlCaption.Ext,
            Url = youTubeDlCaption.Url,
            Name = youTubeDlCaption.Name,
        };

        return domainCaption;
    }

    public static ImageFile ToDalImageFile(this YoutubeExplode.Common.Thumbnail thumbnail)
    {
        return new ImageFile
        {
            Url = thumbnail.Url,

            Width = thumbnail.Resolution.Width,
            Height = thumbnail.Resolution.Height,
        };
    }

    public static ImageFile ToDalImageFile(this ThumbnailData thumbnailData)
    {
        return new ImageFile
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = thumbnailData.ID,
            Url = thumbnailData.Url,
            Width = thumbnailData.Width,
            Height = thumbnailData.Height
        };
    }

    public static Playlist ToDalPlaylist(this VideoData playlistData, ImageFileList? previousThumbNails = null)
    {
        var domainPlaylist = new Playlist
        {
            Platform = EPlatform.YouTube,
            IdOnPlatform = playlistData.ID,

            Title = new LangString(playlistData.Title),
            Description = new LangString(playlistData.Description),

            // Not adding thumbnails from unofficial fetch due to inconsistencies between thumbnail formats
            // Only official API thumbnail URLs will be used
            Thumbnails = previousThumbNails,

            PrivacyStatus = playlistData.Availability.ToDalPrivacyStatus(),
            IsAvailable = true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,

            Monitor = true,
            Download = true,
        };

        return domainPlaylist;
    }
}