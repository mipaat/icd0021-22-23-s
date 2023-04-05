using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistHistoryRepository : BaseEntityRepository<PlaylistHistory, AbstractAppDbContext>, IPlaylistHistoryRepository
{
    public PlaylistHistoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<PlaylistHistory> DefaultIncludes(DbSet<PlaylistHistory> entities)
    {
        entities.Include(ph => ph.Playlist);
        return entities;
    }
}