using App.Common.Enums;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface ICategoryRepository : IBaseEntityRepository<Domain.Category, CategoryWithCreator>
{
    public Task<ICollection<CategoryWithCreator>> GetAllByPlatformAsync(EPlatform platform,
        IEnumerable<string>? idsOnPlatform = null);

    public Task<CategoryWithCreator?> GetByNameAsync(EPlatform platform, string name);
    public Task<ICollection<CategoryWithCreator>> GetAllPublicOrCreatedByAuthorAsync(Guid? creatorId);
    public Task<ICollection<CategoryWithCreator>> GetAllAssignableCategoriesForAuthor(Guid? authorId);

    public Task<ICollection<CategoryWithCreatorAndVideoAssignments>> GetAllByIdsWithVideoAssignments(Guid videoId,
        IEnumerable<Guid> ids, Guid? authorId);
}