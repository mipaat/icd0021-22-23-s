using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoCategoryRepository : BaseAppEntityRepository<App.Domain.VideoCategory, VideoCategory>,
    IVideoCategoryRepository
{
    public VideoCategoryRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    protected override Domain.VideoCategory AfterMap(VideoCategory entity, Domain.VideoCategory mapped)
    {
        var video = Uow.Videos.GetTrackedEntity(entity.VideoId);
        if (video != null)
        {
            if (entity.Video != null)
            {
                mapped.Video = Uow.Videos.Map(entity.Video, video);
            }
            else
            {
                mapped.Video = video;
            }
        }

        var category = Uow.Categories.GetTrackedEntity(entity.CategoryId);
        if (category != null)
        {
            if (entity.Category != null)
            {
                mapped.Category = Uow.Categories.Map(entity.Category, category);
            }
            else
            {
                mapped.Category = category;
            }
        }

        return mapped;
    }

    public async Task<bool> ExistsAsync(Guid categoryId, Guid videoId)
    {
        return await Entities.AnyAsync(e => e.CategoryId == categoryId && e.VideoId == videoId);
    }

    public async Task<ICollection<Guid>> GetAllCategoryIdsAsync(Guid videoId, Guid authorId)
    {
        return await Entities.Where(e => e.VideoId == videoId && e.AssignedById == authorId)
            .Select(e => e.CategoryId)
            .ToListAsync();
    }

    public void RemoveTracked(VideoCategoryOnlyIds entity)
    {
        var tracked = GetTrackedEntity(entity.Id)!;
        Entities.Remove(tracked);
    }
}