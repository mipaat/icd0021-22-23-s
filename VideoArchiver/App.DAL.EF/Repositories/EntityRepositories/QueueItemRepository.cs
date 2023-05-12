using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class QueueItemRepository : BaseAppEntityRepository<App.Domain.QueueItem, QueueItem>, IQueueItemRepository
{
    public QueueItemRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}