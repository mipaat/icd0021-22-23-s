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

    public CategoryService(ServiceUow serviceUow, ILogger<CategoryService> logger, IMapper mapper) : base(serviceUow,
        logger, mapper)
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

    public async Task<ICollection<CategoryWithCreator>> GetAllCategoriesAsync(Guid? authorId)
    {
        return (await Uow.Categories.GetAllPublicOrCreatedByAuthorAsync(authorId)).Select(c => _categoryMapper.Map(c)!)
            .ToList();
    }

    public async Task<ICollection<Guid>> GetAllAssignedCategoryIds(Guid authorId, Guid entityId, EEntityType entityType)
    {
        return entityType switch
        {
            EEntityType.Video => await Uow.VideoCategories.GetAllCategoryIdsAsync(entityId, authorId),
            EEntityType.Playlist => await Uow.PlaylistCategories.GetAllCategoryIdsAsync(entityId, authorId),
            EEntityType.Author => await Uow.AuthorCategories.GetAllCategoryIdsAsync(entityId, authorId),
            _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null),
        };
    }

    public async Task<ICollection<CategoryWithCreator>>
        GetAllCategoriesFilterableForAuthorAsync(Guid? authorId)
    {
        return await GetAllCategoriesAsync(authorId);
    }

    public async Task<ICollection<CategoryWithCreator>> GetAllAssignableCategoriesForAuthor(Guid? authorId)
    {
        return (await Uow.Categories.GetAllAssignableCategoriesForAuthor(authorId))
            .Select(c => _categoryMapper.Map(c)!).ToList();
    }

    public async Task DeleteAsync(Guid id)
    {
        var videoCategories = await Uow.VideoCategories.GetAllAsync(e => e.CategoryId == id);
        foreach (var category in videoCategories)
        {
            Uow.VideoCategories.Remove(category);
        }

        var authorCategories = await Uow.AuthorCategories.GetAllAsync(e => e.CategoryId == id);
        foreach (var category in authorCategories)
        {
            Uow.AuthorCategories.Remove(category);
        }

        var playlistCategories = await Uow.PlaylistCategories.GetAllAsync(e => e.CategoryId == id);
        foreach (var category in playlistCategories)
        {
            Uow.PlaylistCategories.Remove(category);
        }

        await Uow.Categories.RemoveAsync(id);
    }

    private async Task AddVideoToCategories(Guid authorId, Guid videoId, Dictionary<Guid, bool> categoryIds)
    {
        var categories =
            await Uow.Categories.GetAllByIdsWithVideoAssignments(videoId, categoryIds.Keys, authorId);

        foreach (var categoryId in categoryIds)
        {
            var category = categories.First(c => c.Id == categoryId.Key);
            if (categoryId.Value)
            {
                if (category.VideoCategories.Any()) continue;
                Uow.VideoCategories.Add(new VideoCategory
                {
                    AssignedById = authorId,
                    CategoryId = category.Id,
                    VideoId = videoId,
                });
            }
            else
            {
                var videoCategoryOnlyIds = category.VideoCategories.FirstOrDefault();
                if (videoCategoryOnlyIds == null) continue;
                Uow.VideoCategories.RemoveTracked(videoCategoryOnlyIds);
            }
        }
    }

    public async Task AddToCategories(Guid authorId, Guid entityId, EEntityType entityType,
        Dictionary<Guid, bool> categoryIds)
    {
        switch (entityType)
        {
            case EEntityType.Video:
                await AddVideoToCategories(authorId, entityId, categoryIds);
                break;
            default:
                throw new NotImplementedException(); // TODO playlists and authors
        }
    }
}