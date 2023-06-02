using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IEntityAccessPermissionRepository : IBaseEntityRepository<Domain.EntityAccessPermission, EntityAccessPermission>
{
    public Task<bool> AllowedToAccessVideoAsync(Guid userId, Guid videoId);
    public Task<bool> AllowedToAccessVideoAnonymouslyAsync(Guid videoId);
    public Task<bool> VideoPermissionExistsAsync(Guid userId, Guid videoId);
    public Task<bool> PlaylistPermissionExistsAsync(Guid userId, Guid playlistId);
    public Task<bool> AuthorPermissionExistsAsync(Guid userId, Guid authorId);
}