using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistVideoPositionHistoryRepository :
    BaseEntityRepository<PlaylistVideoPositionHistory, AbstractAppDbContext>, IPlaylistVideoPositionHistoryRepository
{
    public PlaylistVideoPositionHistoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<PlaylistVideoPositionHistory> DefaultIncludes(DbSet<PlaylistVideoPositionHistory> entities)
    {
        entities.Include(p => p.PlaylistVideo);
        return entities;
    }
}