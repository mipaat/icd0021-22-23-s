using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistAuthorRepository : BaseAppEntityRepository<App.Domain.PlaylistAuthor, PlaylistAuthor>,
    IPlaylistAuthorRepository
{
    public PlaylistAuthorRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(
        dbContext, mapper, uow)
    {
    }

    protected override TQueryable IncludeDefaults<TQueryable>(TQueryable queryable)
    {
        queryable.Include(e => e.Author)
            .Include(e => e.Playlist);
        return queryable;
    }

    protected override Domain.PlaylistAuthor AfterMap(PlaylistAuthor entity, Domain.PlaylistAuthor mapped)
    {
        var trackedAuthor = Uow.Authors.GetTrackedEntity(entity.AuthorId);
        if (trackedAuthor != null)
        {
            mapped.Author = trackedAuthor;
        }

        var trackedPlaylist = Uow.Playlists.GetTrackedEntity(entity.PlaylistId);
        if (trackedPlaylist != null)
        {
            mapped.Playlist = trackedPlaylist;
        }

        return mapped;
    }

    public async Task SetPlaylistAuthor(Guid playlistId, Guid authorId,
        EAuthorRole authorRole = EAuthorRole.Publisher)
    {
        var playlistAuthors = await GetAllByPlaylistAndAuthor(playlistId, authorId, authorRole);
        if (playlistAuthors.Count > 0) return;

        var playlistAuthor = new PlaylistAuthor
        {
            PlaylistId = playlistId,
            AuthorId = authorId,
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
        if (authorRole != null)
        {
            query = query.Where(pa => pa.Role == authorRole);
        }

        return await query.ProjectTo<PlaylistAuthor>(Mapper.ConfigurationProvider).ToListAsync();
    }
}