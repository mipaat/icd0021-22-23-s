using System.Linq.Expressions;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using App.DAL.DTO.Entities.Identity;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories.Identity;

public class RefreshTokenRepository : BaseAppEntityRepository<App.Domain.Identity.RefreshToken, RefreshToken>,
    IRefreshTokenRepository
{
    public RefreshTokenRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    public async Task<ICollection<RefreshToken>> GetAllByUserIdAsync(Guid userId,
        params Expression<Func<App.Domain.Identity.RefreshToken, bool>>[] filters)
    {
        var newFilters = new List<Expression<Func<App.Domain.Identity.RefreshToken, bool>>>();
        newFilters.Add(rt => rt.UserId == userId);
        newFilters.AddRange(filters);
        return await GetAllAsync(newFilters.ToArray());
    }

    public async Task<ICollection<RefreshToken>> GetAllFullyExpiredByUserIdAsync(Guid userId)
    {
        return (await GetAllByUserIdAsync(userId)).Where(r => r.IsFullyExpired).ToList();
    }

    public async Task<ICollection<RefreshToken>> GetAllValidByUserIdAndRefreshTokenAsync(Guid userId,
        string refreshToken)
    {
        return await GetAllByUserIdAsync(userId, r =>
            (r.RefreshToken == refreshToken && r.ExpiresAt > DateTime.UtcNow) ||
            (r.PreviousRefreshToken == refreshToken && r.PreviousExpiresAt > DateTime.UtcNow));
    }
}