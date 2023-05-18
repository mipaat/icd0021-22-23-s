using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using App.DAL.DTO.Entities.Identity;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories.Identity;

public class UserRepository : BaseAppEntityRepository<App.Domain.Identity.User, User>, IUserRepository
{
    public UserRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper,
        uow)
    {
    }

    public async Task<ICollection<UserWithRoles>> GetAllWithRoles(bool includeOnlyRequiringApproval = false,
        string? nameQuery = null)
    {
        IQueryable<App.Domain.Identity.User> query = Entities
            .Include(e => e.UserRoles!)
            .ThenInclude(e => e.Role);
        if (includeOnlyRequiringApproval)
        {
            query = query.Where(e => !e.IsApproved);
        }

        if (nameQuery != null)
        {
            nameQuery = "%" + nameQuery.ToUpper() + "%";
            query = query.Where(e => e.NormalizedUserName != null &&
                                     Microsoft.EntityFrameworkCore.EF.Functions.Like(e.NormalizedUserName, nameQuery));
        }

        return await query.ProjectTo<UserWithRoles>(Mapper.ConfigurationProvider).ToListAsync();
    }

    public Task<bool> IsInRoleAsync(Guid userid, Guid roleId)
    {
        return DbContext.UserRoles.AnyAsync(e => e.UserId == userid && e.RoleId == roleId);
    }

    public void AddToRoles(Guid userId, params Guid[] roleIds)
    {
        if (roleIds.Length == 0) return;
        var trackedUser = DbContext.Users.GetTrackedEntity(userId);
        foreach (var roleId in roleIds)
        {
            var trackedRole = DbContext.Roles.GetTrackedEntity(roleId);
            DbContext.UserRoles.Add(new App.Domain.Identity.UserRole
            {
                UserId = userId,
                User = trackedUser,
                RoleId = roleId,
                Role = trackedRole,
            });
        }
    }

    public async Task RemoveFromRolesAsync(Guid userId, params Guid[] roleIds)
    {
        if (roleIds.Length == 0) return;
        await DbContext.UserRoles.Where(e => e.UserId == userId && roleIds.Contains(e.RoleId)).ExecuteDeleteAsync();
    }
}