using App.Common.Enums;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface ICategoryRepository : IBaseEntityRepository<App.Domain.Category, CategoryWithCreator>
{
    public Task<ICollection<CategoryWithCreator>> GetAllByPlatformAsync(EPlatform platform, IEnumerable<string>? idsOnPlatform = null);
    public Task<CategoryWithCreator?> GetByNameAsync(EPlatform platform, string name);
    public Task<ICollection<CategoryWithCreator>> GetAllAsync(Guid? creatorId);
}