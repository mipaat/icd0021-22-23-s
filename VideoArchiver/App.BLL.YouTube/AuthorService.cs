using App.Domain;
using App.Domain.Enums;

namespace App.BLL.YouTube;

public class AuthorService : BaseYouTubeService
{
    public AuthorService(YouTubeUow youTubeUow) : base(youTubeUow)
    {
    }

    public async Task AddAndSetAuthorIfNotSet(Video domainVideo, YoutubeExplode.Common.Author youTubeExplodeAuthor)
    {
        var domainAuthor = await AddOrGetAuthor(youTubeExplodeAuthor);
        await Uow.VideoAuthors.SetVideoAuthor(domainVideo, domainAuthor);
    }

    public async Task AddAndSetAuthorIfNotSet(Playlist domainPlaylist,
        YoutubeExplode.Common.Author youTubeExplodeAuthor)
    {
        var domainAuthor = await AddOrGetAuthor(youTubeExplodeAuthor);
        await Uow.PlaylistAuthors.SetPlaylistAuthor(domainPlaylist, domainAuthor);
    }

    public async Task<Author> AddOrGetAuthor(YoutubeExplode.Common.Author youTubeExplodeAuthor)
    {
        var author = await Uow.Authors.GetByIdOnPlatformAsync(youTubeExplodeAuthor.ChannelId, Platform.YouTube);
        if (author == null)
        {
            author = youTubeExplodeAuthor.ToDomainAuthor();
            Uow.Authors.Add(author);
        }

        return author;
    }
}