using App.BLL.Base;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using App.Common.Enums;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using CategoryWithCreator = App.BLL.DTO.Entities.CategoryWithCreator;
using Video = App.DAL.DTO.Entities.Video;

namespace App.BLL.Services;

public class CategoryService : BaseService<CategoryService>
{
    private readonly CategoryMapper _categoryMapper;
    
    public CategoryService(ServiceUow serviceUow, ILogger<CategoryService> logger, IMapper mapper) : base(serviceUow, logger, mapper)
    {
        _categoryMapper = new CategoryMapper(mapper);
    }

    public async Task AddToCategory(Video video, DAL.DTO.Entities.CategoryWithCreator category)
    {
        if (await Uow.VideoCategories.ExistsAsync(category.Id, video.Id)) return;
        Uow.VideoCategories.Add(new VideoCategory
        {
            Video = video,
            VideoId = video.Id,
            Category = category,
            CategoryId = category.Id,
        });
    }

    public Guid CreateCategory(CategoryDataWithCreatorId categoryData)
    {
        var category = _categoryMapper.Map(categoryData);
        category.Platform = EPlatform.This;
        category.IsAssignable = true;
        
        Uow.Categories.Add(category);

        return category.Id;
    }

    public async Task<CategoryWithCreator?> GetByIdAsync(Guid categoryId)
    {
        return _categoryMapper.Map(await Uow.Categories.GetByIdAsync(categoryId));
    }

    public void Update(Guid id, CategoryData data)
    {
        Uow.Categories.Update(_categoryMapper.Map(data, id));
    }

    public async Task<ICollection<CategoryWithCreator>> GetAllCategoriesAsync(Guid? userId)
    {
        return (await Uow.Categories.GetAllAsync(userId)).Select(c => _categoryMapper.Map(c)!).ToList();
    }

    public async Task<Dictionary<EPlatform, ICollection<CategoryWithCreator>>>
        GetAllCategoriesGroupedByPlatformAsync(Guid? userId)
    {
        var categories = await GetAllCategoriesAsync(userId);
        var result = new Dictionary<EPlatform, ICollection<CategoryWithCreator>>();
        foreach (var category in categories)
        {
            result.TryAdd(category.Platform, new List<CategoryWithCreator>());
            result[category.Platform].Add(category);
        }

        return result;
    }
}