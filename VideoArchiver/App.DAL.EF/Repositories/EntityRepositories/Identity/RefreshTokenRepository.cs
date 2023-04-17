using System.Linq.Expressions;
using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using DAL.Base;
using Domain.Identity;

namespace DAL.Repositories.EntityRepositories.Identity;

public class RefreshTokenRepository : BaseEntityRepository<RefreshToken, AbstractAppDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<RefreshToken>> GetAllByUserIdAsync(Guid userId,
        params Expression<Func<RefreshToken, bool>>[] filters)
    {
        return await GetAllAsync(rt => rt.UserId == userId);
    }
}