using App.DAL.DTO.Entities.Identity;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories.Identity;

public interface IUserRepository : IBaseEntityRepository<App.Domain.Identity.User, User>
{
    public Task<ICollection<UserWithRoles>> GetAllWithRoles(bool includeOnlyRequiringApproval = false, string? nameQuery = null);
    public Task<UserWithRoles?> GetByIdWithRolesAsync(Guid id);
    public Task<bool> IsInRoleAsync(Guid userid, Guid roleId);
    public void AddToRoles(Guid userId, params Guid[] roleIds);
    public Task RemoveFromRolesAsync(Guid userId, params Guid[] roleIds);
}