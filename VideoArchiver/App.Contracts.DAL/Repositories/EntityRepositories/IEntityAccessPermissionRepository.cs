using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IEntityAccessPermissionRepository : IBaseEntityRepository<Domain.EntityAccessPermission, EntityAccessPermission>
{
    public Task<bool> AllowedToAccessVideoAsync(Guid userId, Guid videoId);
    public Task<bool> AllowedToAccessVideoAnonymouslyAsync(Guid videoId);
}