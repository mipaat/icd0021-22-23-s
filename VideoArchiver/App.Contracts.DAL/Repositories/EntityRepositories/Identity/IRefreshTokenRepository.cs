using System.Linq.Expressions;
using Contracts.DAL;
using Domain.Identity;

namespace App.Contracts.DAL.Repositories.EntityRepositories.Identity;

public interface IRefreshTokenRepository : IBaseEntityRepository<RefreshToken>
{
    public Task<ICollection<RefreshToken>> GetAllByUserIdAsync(Guid userId, params Expression<Func<RefreshToken, bool>>[] filters);
}