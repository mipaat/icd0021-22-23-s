using App.Common.Enums;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface ICategoryRepository : IBaseEntityRepository<App.Domain.Category, Category>
{
    public Task<ICollection<Category>> GetAllByPlatformAsync(EPlatform platform, IEnumerable<string>? idsOnPlatform = null);
    public Task<Category?> GetByNameAsync(EPlatform platform, string name);
}