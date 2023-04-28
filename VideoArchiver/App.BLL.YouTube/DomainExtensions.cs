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
}