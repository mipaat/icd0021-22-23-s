using App.BLL.Base;
using App.BLL.DTO.Exceptions;
using App.BLL.DTO.Mappers;
using App.Common;
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

    public async Task<CategoryWithCreator> GetCategoryAsync(Guid id)
    {
        var category = await Uow.Categories.GetByIdAsync(id) ?? throw new NotFoundException();
        return _categoryMapper.Map(category)!;
    }

    public Guid CreateCategory(LangString name, bool isPublic, Guid authorId)
    {
        var category = new DAL.DTO.Entities.CategoryWithCreator
        {
            CreatorId = authorId,
            IsAssignable = true,
            IsPublic = isPublic,
            Platform = EPlatform.This,
            Name = name,
        };
        Uow.Categories.Add(category);

        return category.Id;
    }
}