using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities.Playlists;
using App.Common.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistRepository : BaseAppEntityRepository<App.Domain.Playlist, Playlist>,
    IPlaylistRepository
{
    public PlaylistRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) :
        base(dbContext, mapper, uow)
    {
    }

    public async Task<Playlist?> GetByIdOnPlatformAsync(string idOnPlatform, EPlatform platform)
    {
        return AttachIfNotAttached(await Entities
            .Include(pl => pl.PlaylistAuthors!)
            .ThenInclude(pa => pa.Author)
            .Where(pl => pl.IdOnPlatform == idOnPlatform && pl.Platform == platform)
            .ProjectTo<Playlist>(Mapper.ConfigurationProvider)
            .SingleOrDefaultAsync());
    }

    public async Task<ICollection<Playlist>> GetAllNotOfficiallyFetched(EPlatform platform, int? limit = null)
    {
        IQueryable<App.Domain.Playlist> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable && v.LastSuccessfulFetchOfficial == null)
            .OrderBy(v => v.AddedToArchiveAt);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return AttachIfNotAttached<ICollection<Playlist>, Playlist>(await query.ProjectTo<Playlist>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ICollection<Playlist>> GetAllBeforeOfficialApiFetch(EPlatform platform, DateTime cutoff,
        int? limit = null)
    {
        IQueryable<App.Domain.Playlist> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable &&
                        v.LastFetchOfficial != null && v.LastFetchOfficial < cutoff)
            .OrderBy(v => v.LastFetchOfficial);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return AttachIfNotAttached<ICollection<Playlist>, Playlist>(
            await query.ProjectTo<Playlist>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ICollection<PlaylistWithBasicVideoData>> GetAllWithContentsUpdatedBefore(EPlatform platform,
        DateTime cutoff, int? limit = null)
    {
        IQueryable<App.Domain.Playlist> query = Entities
            .Where(e => e.Platform == platform && e.IsAvailable && e.LastVideosFetch < cutoff)
            .OrderBy(e => e.LastVideosFetch)
            .Include(e => e.PlaylistVideos!.OrderBy(c => c.Position))
            .ThenInclude(e => e.Video);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return AttachIfNotAttached<ICollection<PlaylistWithBasicVideoData>, PlaylistWithBasicVideoData>(
            await query.ProjectTo<PlaylistWithBasicVideoData>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public void UpdateContents(PlaylistWithBasicVideoData playlist)
    {
        _ = GetTrackedEntity(playlist.Id) ??
            Entities.Update(Mapper.Map<PlaylistWithBasicVideoData, App.Domain.Playlist>(playlist)!).Entity;
        foreach (var playlistVideo in playlist.PlaylistVideos)
        {
            var trackedPlaylistVideo = Uow.PlaylistVideos.GetTrackedEntity(playlistVideo.Id);
            if (trackedPlaylistVideo == null)
            {
                Uow.PlaylistVideos.Add(playlistVideo, playlist.Id);
            }
            else
            {
                Uow.PlaylistVideos.Update(playlistVideo);
            }
        }
    }
}