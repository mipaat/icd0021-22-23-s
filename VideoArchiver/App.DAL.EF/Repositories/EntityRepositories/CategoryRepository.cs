using App.Common.Enums;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class CategoryRepository : BaseAppEntityRepository<Domain.Category, CategoryWithCreator>, ICategoryRepository
{
    public CategoryRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext,
        mapper, uow)
    {
    }

    protected override Domain.Category AfterMap(CategoryWithCreator entity, Domain.Category mapped)
    {
        if (entity.Creator != null)
        {
            var creator = Uow.Authors.GetTrackedEntity(entity.Creator.Id);
            if (creator != null)
            {
                mapped.Creator = creator;
            }
        }

        return mapped;
    }

    public async Task<ICollection<CategoryWithCreator>> GetAllByPlatformAsync(EPlatform platform,
        IEnumerable<string>? idsOnPlatform = null)
    {
        var query = Entities.Where(e => e.Platform == platform);
        if (idsOnPlatform != null)
        {
            query = query.Where(e => idsOnPlatform.Contains(e.IdOnPlatform));
        }

        return AttachIfNotAttached<ICollection<CategoryWithCreator>, CategoryWithCreator>(
            await query.ProjectTo<CategoryWithCreator>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<CategoryWithCreator?> GetByNameAsync(EPlatform platform, string name)
    {
        return AttachIfNotAttached(await Entities.FromSql(
                $"SELECT * FROM \"Categories\" c WHERE jsonb_path_exists(c.\"Name\", ('$.* ? (@ like_regex \"(?i)' || {name} || '\")')::jsonpath)")
            .Where(e => e.Platform == platform)
            .ProjectTo<CategoryWithCreator>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync());
    }

    public async Task<ICollection<CategoryWithCreator>> GetAllPublicOrCreatedByAuthorAsync(Guid? creatorId)
    {
        var query = creatorId == null
            ? Entities.Where(e => e.IsPublic)
            : Entities.Where(e => e.IsPublic || e.CreatorId == creatorId);
        return AttachIfNotAttached<ICollection<CategoryWithCreator>, CategoryWithCreator>(
            await query.ProjectTo<CategoryWithCreator>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public Task<ICollection<CategoryWithCreator>> GetAllAssignableCategoriesForAuthor(Guid? authorId)
    {
        return GetAllAsync(c =>
            c.Platform == EPlatform.This && c.IsAssignable &&
            ((authorId != null && c.CreatorId == authorId) || c.IsPublic));
    }

    public async Task<ICollection<CategoryWithCreatorAndVideoAssignments>> GetAllByIdsWithVideoAssignments(Guid videoId,
        IEnumerable<Guid> ids, Guid? authorId)
    {
        return AttachIfNotAttached<ICollection<CategoryWithCreatorAndVideoAssignments>,
            CategoryWithCreatorAndVideoAssignments>(
            await Entities
                .Include(c => c.VideoCategories)
                .Include(c => c.Creator)
                .Where(c => ids.Contains(c.Id) && c.Platform == EPlatform.This && c.IsAssignable)
                .Select(c => new CategoryWithCreatorAndVideoAssignments
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPublic = c.IsPublic,
                    IsAssignable = c.IsAssignable,
                    Platform = c.Platform,
                    IdOnPlatform = c.IdOnPlatform,
                    CreatorId = c.CreatorId,
                    Creator = Mapper.Map<AuthorBasic>(c.Creator),
                    VideoCategories = c.VideoCategories!
                        .Where(e => e.AssignedById == authorId && e.VideoId == videoId)
                        .Select(vc => Mapper.Map<VideoCategoryOnlyIds>(vc))
                        .ToList()
                })
                .ToListAsync());
    }
}