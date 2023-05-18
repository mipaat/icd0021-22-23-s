using App.BLL.Base;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class CategoryService : BaseService<CategoryService>
{
    public CategoryService(ServiceUow serviceUow, ILogger<CategoryService> logger, IMapper mapper) : base(serviceUow, logger, mapper)
    {
    }

    public async Task AddToCategory(Video video, Category category)
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
}