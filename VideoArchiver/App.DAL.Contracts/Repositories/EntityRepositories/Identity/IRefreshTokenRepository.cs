using System.Linq.Expressions;
using App.Domain.Identity;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories.Identity;

public interface IRefreshTokenRepository : IBaseEntityRepository<RefreshToken, App.DAL.DTO.Entities.Identity.RefreshToken>
{
    public Task<ICollection<App.DAL.DTO.Entities.Identity.RefreshToken>> GetAllByUserIdAsync(Guid userId, params Expression<Func<RefreshToken, bool>>[] filters);
    public Task<ICollection<App.DAL.DTO.Entities.Identity.RefreshToken>> GetAllFullyExpiredByUserIdAsync(Guid userId);

    public Task<ICollection<App.DAL.DTO.Entities.Identity.RefreshToken>> GetAllValidByUserIdAndRefreshTokenAsync(
        Guid userId, string refreshToken);
}