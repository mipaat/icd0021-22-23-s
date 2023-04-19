using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistVideoRepository : BaseEntityRepository<PlaylistVideo, AbstractAppDbContext>,
    IPlaylistVideoRepository
{
    public PlaylistVideoRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<PlaylistVideo> DefaultIncludes(DbSet<PlaylistVideo> entities)
    {
        entities
            .Include(p => p.AddedBy)
            .Include(p => p.Playlist)
            .Include(p => p.RemovedBy)
            .Include(p => p.Video);
        return entities;
    }
}