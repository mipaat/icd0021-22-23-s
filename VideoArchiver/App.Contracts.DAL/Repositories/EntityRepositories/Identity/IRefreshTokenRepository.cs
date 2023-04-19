using System.Linq.Expressions;
using App.Domain.Identity;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories.Identity;

public interface IRefreshTokenRepository : IBaseEntityRepository<RefreshToken>
{
    public Task<ICollection<RefreshToken>> GetAllByUserIdAsync(Guid userId, params Expression<Func<RefreshToken, bool>>[] filters);
}