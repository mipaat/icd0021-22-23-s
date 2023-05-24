using App.BLL.DTO.Contracts;
using App.BLL.DTO.Entities;
using App.BLL.YouTube.Utils;
using App.Common.Enums;

namespace App.BLL.YouTube.Services;

public class PresentationHandler : IPlatformVideoPresentationHandler, IPlatformAuthorPresentationHandler
{
    public bool CanHandle(Video video)
    {
        return video.Platform == EPlatform.YouTube;
    }

    public bool CanHandle(BasicVideoWithAuthor video)
    {
        return video.Platform == EPlatform.YouTube;
    }

    public void Handle(VideoWithAuthorAndComments video)
    {
        video.Url = Url.ToVideoUrl(video.IdOnPlatform);
        video.EmbedUrl = Url.ToVideoEmbedUrl(video.IdOnPlatform);
        video.Author = Handle(video.Author);
    }

    public void Handle(BasicVideoWithAuthor video)
    {
        if (video.Thumbnails != null)
        {
            var thumbnails = video.Thumbnails
                .OrderByDescending(i => i.Quality, new ThumbnailQualityComparer())
                .ThenByDescending(i => i.Key, new ThumbnailTagComparer());
            video.Thumbnail = thumbnails.FirstOrDefault();
        }

        video.Author = Handle(video.Author);
    }

    public bool CanHandle(Author author)
    {
        return author.Platform == EPlatform.YouTube;
    }

    public Author Handle(Author author)
    {
        author.UrlOnPlatform = Url.ToAuthorUrl(author.IdOnPlatform);
        return author;
    }
}