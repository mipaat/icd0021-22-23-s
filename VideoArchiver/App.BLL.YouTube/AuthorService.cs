using App.Domain;
using App.Domain.Enums;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube;

public class AuthorService : BaseYouTubeService
{
    public AuthorService(YouTubeUow youTubeUow) : base(youTubeUow)
    {
    }

    public async Task AddAndSetAuthorIfNotSet(Video domainVideo, VideoData videoData)
    {
        var domainAuthor = await AddOrGetAuthor(videoData);
        await Uow.VideoAuthors.SetVideoAuthor(domainVideo, domainAuthor);
    }

    public async Task AddAndSetAuthorIfNotSet(Comment domainComment, CommentData commentData)
    {
        var domainAuthor = await AddOrGetAuthor(commentData);
        domainComment.Author = domainAuthor;
    }

    private async Task<Author> AddOrGetAuthor(VideoData videoData)
    {
        var author = await Uow.Authors.GetByIdOnPlatformAsync(videoData.ChannelID, Platform.YouTube);
        if (author == null)
        {
            author = videoData.ToDomainAuthor();
            Uow.Authors.Add(author);
        }

        return author;
    }

    public async Task<Author> AddOrGetAuthor(CommentData commentData)
    {
        var author = await Uow.Authors.GetByIdOnPlatformAsync(commentData.AuthorID, Platform.YouTube);
        if (author == null)
        {
            author = commentData.ToDomainAuthor();
            Uow.Authors.Add(author);
        }

        return author;
    }
}