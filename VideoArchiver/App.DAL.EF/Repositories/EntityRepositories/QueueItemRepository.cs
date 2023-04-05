using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class QueueItemRepository : BaseEntityRepository<QueueItem, AbstractAppDbContext>, IQueueItemRepository
{
    public QueueItemRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<QueueItem> DefaultIncludes(DbSet<QueueItem> entities)
    {
        entities
            .Include(q => q.AddedBy)
            .Include(q => q.ApprovedBy)
            .Include(q => q.Author)
            .Include(q => q.Playlist)
            .Include(q => q.Video);
        return entities;
    }
}