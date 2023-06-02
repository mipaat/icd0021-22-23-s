using App.BLL.DTO.Entities;
using App.Common.Enums;
using CategoryWithCreator = App.BLL.DTO.Entities.CategoryWithCreator;
using Video = App.DAL.DTO.Entities.Video;

namespace App.BLL.Contracts.Services;

public interface ICategoryService : IBaseService
{
    public Task AddToCategory(Video video, DAL.DTO.Entities.CategoryWithCreator category);
    public Guid CreateCategory(CategoryDataWithCreatorId categoryData);
    public Task<CategoryWithCreator?> GetByIdAsync(Guid categoryId);
    public void Update(Guid id, CategoryData data);
    public Task<ICollection<Guid>> GetAllAssignedCategoryIds(Guid? authorId, Guid entityId, EEntityType entityType);

    public Task<ICollection<CategoryWithCreator>>
        GetAllCategoriesFilterableForAuthorAsync(Guid? authorId);

    public Task<List<CategoryWithCreator>> GetAllAssignableCategoriesForAuthor(Guid? authorId);
    public Task DeleteAsync(Guid id);

    public Task AddToCategories(Guid? authorId, Guid entityId, EEntityType entityType,
        Dictionary<Guid, bool> categoryIds);
}