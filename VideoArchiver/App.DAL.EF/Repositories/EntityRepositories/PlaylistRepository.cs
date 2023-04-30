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
}