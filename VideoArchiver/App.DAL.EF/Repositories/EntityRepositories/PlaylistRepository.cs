using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class PlaylistRepository : BaseAppEntityRepository<App.Domain.Playlist, Playlist>, IPlaylistRepository
{
    public PlaylistRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    public async Task<Playlist?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform)
    {
        return Mapper.Map(await Entities
            .Include(pl => pl.PlaylistAuthors!)
            .ThenInclude(pa => pa.Author)
            .Where(pl => pl.IdOnPlatform == idOnPlatform && pl.Platform == platform)
            .SingleOrDefaultAsync());
    }

    public async Task<ICollection<Playlist>> GetAllNotOfficiallyFetched(Platform platform, int? limit = null)
    {
        IQueryable<App.Domain.Playlist> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable && v.LastSuccessfulFetchOfficial == null)
            .OrderBy(v => v.AddedToArchiveAt);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return (await query.ToListAsync()).Select(e => Mapper.Map(e)!).ToList();
    }

    public async Task<ICollection<Playlist>> GetAllBeforeOfficialApiFetch(Platform platform, DateTime cutoff,
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

        return (await query.ToListAsync()).Select(e => Mapper.Map(e)!).ToList();
    }
}