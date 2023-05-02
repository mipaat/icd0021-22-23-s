using App.Domain.Enums;
using App.Domain.NotMapped;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube;

public static class DomainExtensions
{
    public static Domain.Video ToDomainVideo(this VideoData videoData)
    {
        var domainVideo = new Domain.Video
        {
            Platform = Platform.YouTube,
            IdOnPlatform = videoData.ID,

            Title = new LangString(videoData.Title, LangString.UnknownCulture),
            Description = new LangString(videoData.Description, LangString.UnknownCulture),

            Duration = videoData.Duration != null ? TimeSpan.FromSeconds(videoData.Duration.Value) : null,

            ViewCount = videoData.ViewCount,
            LikeCount = videoData.LikeCount,
            DislikeCount = videoData.DislikeCount,
            CommentCount = videoData.CommentCount,

            Captions = videoData.Subtitles.ToDomainCaptions(),
            AutomaticCaptions = videoData.AutomaticCaptions.ToDomainCaptions(),
            Thumbnails = videoData.Thumbnails.Select(t => t.ToDomainImageFile()).ToList(),
            Tags = videoData.Tags.ToList(),

            IsLivestreamRecording = videoData.WasLive ?? videoData.IsLive,
            LivestreamStartedAt = (videoData.WasLive ?? videoData.IsLive) == true ? videoData.ReleaseTimestamp : null,

            CreatedAt = videoData.UploadDate,
            PublishedAt = videoData.ReleaseTimestamp,
            UpdatedAt = videoData.ModifiedTimestamp,

            PrivacyStatus = videoData.Availability.ToDomainPrivacyStatus(),
            IsAvailable = videoData.Availability.ToDomainPrivacyStatus().IsAvailable(),

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,
        };

        return domainVideo;
    }

    public static Domain.Author ToDomainAuthor(this VideoData videoData, bool monitor = false, bool download = false)
    {
        var domainAuthor = new Domain.Author
        {
            Platform = Platform.YouTube,
            IdOnPlatform = videoData.ChannelID,

            UserName = videoData.Uploader,
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

    public static Domain.Author ToDomainAuthor(this CommentData commentData)
    {
        var domainAuthor = new Domain.Author
        {
            Platform = Platform.YouTube,
            IdOnPlatform = commentData.AuthorID,

            DisplayName = commentData.Author,

            IsAvailable = true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            ProfileImages = new List<ImageFile>
            {
                new()
                {
                    Platform = Platform.YouTube,
                    Url = commentData.AuthorThumbnail,
                }
            },

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,
        };

        return domainAuthor;
    }

    public static Domain.Comment ToDomainComment(this CommentData commentData)
    {
        return new Domain.Comment
        {
            Platform = Platform.YouTube,
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

    public static CaptionsDictionary ToDomainCaptions(
        this Dictionary<string, SubtitleData[]> youTubeDlCaptions)
    {
        var domainCaptions = new CaptionsDictionary();
        foreach (var key in youTubeDlCaptions.Keys)
        {
            domainCaptions[key] = youTubeDlCaptions[key]
                .Select(youTubeDlCaption => youTubeDlCaption.ToDomainCaption()).ToList();
        }

        return domainCaptions;
    }

    public static EPrivacyStatus? ToDomainPrivacyStatus(this Availability? availability)
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

    public static Caption ToDomainCaption(this SubtitleData youTubeDlCaption)
    {
        var domainCaption = new Caption
        {
            Platform = Platform.YouTube,
            Ext = youTubeDlCaption.Ext,
            Url = youTubeDlCaption.Url,
            Name = youTubeDlCaption.Name,
        };

        return domainCaption;
    }

    public static ImageFile ToDomainImageFile(this YoutubeExplode.Common.Thumbnail thumbnail)
    {
        return new ImageFile
        {
            Url = thumbnail.Url,

            Width = thumbnail.Resolution.Width,
            Height = thumbnail.Resolution.Height,
        };
    }

    public static ImageFile ToDomainImageFile(this ThumbnailData thumbnailData)
    {
        return new ImageFile
        {
            Platform = Platform.YouTube,
            IdOnPlatform = thumbnailData.ID,
            Url = thumbnailData.Url,
            Width = thumbnailData.Width,
            Height = thumbnailData.Height
        };
    }

    public static Domain.Playlist ToDomainPlaylist(this YoutubeExplode.Playlists.Playlist youTubePlaylist,
        bool monitor = false, bool download = false)
    {
        var domainPlaylist = new Domain.Playlist
        {
            Platform = Platform.YouTube,
            IdOnPlatform = youTubePlaylist.Id,

            Title = new LangString(youTubePlaylist.Title, LangString.UnknownCulture),
            Description = new LangString(youTubePlaylist.Description, LangString.UnknownCulture),

            Thumbnails = youTubePlaylist.Thumbnails.Select(t => t.ToDomainImageFile()).ToList(),

            IsAvailable = true,
            InternalPrivacyStatus = EPrivacyStatus.Private,

            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,

            Monitor = monitor,
            Download = download,
        };

        return domainPlaylist;
    }
}