using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IQueueItemRepository : IBaseEntityRepository<QueueItem, App.DAL.DTO.Entities.QueueItem>
{
}