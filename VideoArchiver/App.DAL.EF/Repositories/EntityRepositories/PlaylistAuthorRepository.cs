using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistAuthorRepository : BaseAppEntityRepository<App.Domain.PlaylistAuthor, PlaylistAuthor>,
    IPlaylistAuthorRepository
{
    public PlaylistAuthorRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task SetPlaylistAuthor(Playlist playlist, Author author,
        EAuthorRole authorRole = EAuthorRole.Publisher)
    {
        var playlistAuthors = await GetAllByPlaylistAndAuthor(playlist, author, authorRole);
        if (playlistAuthors.Count > 0) return;

        var playlistAuthor = new PlaylistAuthor
        {
            Playlist = playlist,
            PlaylistId = playlist.Id,
            Author = author,
            AuthorId = author.Id,
            Role = authorRole,
        };

        Add(playlistAuthor);
    }

    public async Task<ICollection<PlaylistAuthor>> GetAllByPlaylistAndAuthor(Playlist playlist, Author author,
        EAuthorRole? authorRole = null)
    {
        return await GetAllByPlaylistAndAuthor(playlist.Id, author.Id, authorRole);
    }

    public async Task<ICollection<PlaylistAuthor>> GetAllByPlaylistAndAuthor(Guid playlistId, Guid authorId,
        EAuthorRole? authorRole = null)
    {
        var query = Entities.Where(pa => pa.PlaylistId == playlistId && pa.AuthorId == authorId);
        var domainAuthorRole = authorRole?.ToDomainAuthorRole();
        if (authorRole != null)
        {
            query = query.Where(pa => pa.Role == domainAuthorRole);
        }

        return (await query.ToListAsync()).Select(pa => Mapper.Map(pa)!).ToList();
    }
}