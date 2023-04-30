using App.Domain;
using App.Domain.Enums;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IAuthorRepository : IBaseEntityRepository<Author>
{
    public Task<Author?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform);
}