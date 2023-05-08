using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using App.Domain.Enums;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistRepository : BaseEntityRepository<Playlist, AbstractAppDbContext>, IPlaylistRepository
{
    public PlaylistRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Playlist?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform)
    {
        return await Entities
            .Include(pl => pl.PlaylistAuthors!)
            .ThenInclude(pa => pa.Author)
            .Where(pl => pl.IdOnPlatform == idOnPlatform && pl.Platform == platform)
            .SingleOrDefaultAsync();
    }

    public async Task<ICollection<Playlist>> GetAllNotOfficiallyFetched(Platform platform, int? limit = null)
    {
        IQueryable<Playlist> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable && v.LastSuccessfulFetchOfficial == null)
            .OrderBy(v => v.AddedToArchiveAt);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<ICollection<Playlist>> GetAllBeforeOfficialApiFetch(Platform platform, DateTime cutoff, int? limit = null)
    {
        IQueryable<Playlist> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable &&
                        v.LastFetchOfficial != null && v.LastFetchOfficial < cutoff)
            .OrderBy(v => v.LastFetchOfficial);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync();
    }
}