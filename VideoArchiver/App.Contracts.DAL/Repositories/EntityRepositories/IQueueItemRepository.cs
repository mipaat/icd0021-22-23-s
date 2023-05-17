using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IQueueItemRepository : IBaseEntityRepository<App.Domain.QueueItem, QueueItem>
{
    public Task<ICollection<QueueItem>> GetAllAwaitingApprovalAsync();
    public Task<ICollection<Guid>> GetAllApprovedNotCompletedAsync();
}