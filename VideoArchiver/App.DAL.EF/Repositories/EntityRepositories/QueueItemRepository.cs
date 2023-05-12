using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class QueueItemRepository : BaseAppEntityRepository<App.Domain.QueueItem, QueueItem>, IQueueItemRepository
{
    public QueueItemRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}