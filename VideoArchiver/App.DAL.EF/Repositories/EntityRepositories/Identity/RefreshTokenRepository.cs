using System.Linq.Expressions;
using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using App.Domain.Identity;
using DAL.Base;

namespace DAL.Repositories.EntityRepositories.Identity;

public class RefreshTokenRepository : BaseEntityRepository<RefreshToken, AbstractAppDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<RefreshToken>> GetAllByUserIdAsync(Guid userId,
        params Expression<Func<RefreshToken, bool>>[] filters)
    {
        var newFilters = new List<Expression<Func<RefreshToken, bool>>>();
        newFilters.Add(rt => rt.UserId == userId);
        newFilters.AddRange(filters);
        return await GetAllAsync(newFilters.ToArray());
    }
}