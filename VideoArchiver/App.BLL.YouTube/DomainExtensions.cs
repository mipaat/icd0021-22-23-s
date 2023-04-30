using App.Domain.Enums;
using App.Domain.NotMapped;

namespace App.BLL.YouTube;

public static class DomainExtensions
{
    public static Domain.Video ToDomainVideo(this YoutubeExplode.Videos.Video youTubeVideo)
    {
        var domainVideo = new Domain.Video
        {
            Platform = Platform.YouTube,
            IdOnPlatform = youTubeVideo.Id,

            Title = new LangString(youTubeVideo.Title, LangString.UnknownCulture),
            Description = new LangString(youTubeVideo.Description, LangString.UnknownCulture),

            Duration = youTubeVideo.Duration,

            ViewCount = youTubeVideo.Engagement.ViewCount,
            LikeCount = youTubeVideo.Engagement.LikeCount,
            DislikeCount = youTubeVideo.Engagement.DislikeCount,

            Thumbnails = youTubeVideo.Thumbnails.Select(t => t.ToDomainImageFile()).ToList(),
            Tags = youTubeVideo.Keywords.ToList(),

            PublishedAt = youTubeVideo.UploadDate.UtcDateTime,
            
            PrivacyStatus = EPrivacyStatus.Public,
            IsAvailable = true,
            InternalPrivacyStatus = EPrivacyStatus.Private,
            
            LastFetchUnofficial = DateTime.UtcNow,
            LastSuccessfulFetchUnofficial = DateTime.UtcNow,
            AddedToArchiveAt = DateTime.UtcNow,
        };

        return domainVideo;
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

    public static Domain.Author ToDomainAuthor(this YoutubeExplode.Common.Author youTubeAuthor, bool monitor = false, bool download = false)
    {
        var domainAuthor = new Domain.Author
        {
            Platform = Platform.YouTube,
            IdOnPlatform = youTubeAuthor.ChannelId,
            
            DisplayName = youTubeAuthor.ChannelTitle,
            
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