using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistAuthorRepository : BaseAppEntityRepository<App.Domain.PlaylistAuthor, PlaylistAuthor>,
    IPlaylistAuthorRepository
{
    public PlaylistAuthorRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(
        dbContext, mapper, uow)
    {
    }

    protected override Func<TQueryable, TQueryable> IncludeDefaultsFunc<TQueryable>()
    {
        return q =>
        {
            q
                .Include(e => e.Author)
                .Include(e => e.Playlist);
            return q;
        };
    }

    protected override Domain.PlaylistAuthor AfterMap(PlaylistAuthor entity, Domain.PlaylistAuthor mapped)
    {
        if (entity.Author != null)
        {
            var trackedAuthor = Uow.Authors.GetTrackedEntity(entity.Author);
            if (trackedAuthor != null)
            {
                mapped.Author = Uow.Authors.Map(entity.Author, trackedAuthor);
            }
            else
            {
                mapped.Author = Uow.Authors.Map(entity.Author);
            }
        }

        if (entity.Playlist != null)
        {
            var trackedPlaylist = Uow.Playlists.GetTrackedEntity(entity.Playlist);
            if (trackedPlaylist != null)
            {
                mapped.Playlist = Uow.Playlists.Map(entity.Playlist, trackedPlaylist);
            }
            else
            {
                mapped.Playlist = Uow.Playlists.Map(entity.Playlist);
            }
        }

        return mapped;
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