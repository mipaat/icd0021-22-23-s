using App.Common;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.Common.Enums;
using App.DAL.EF.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoRepository : BaseAppEntityRepository<App.Domain.Video, Video>, IVideoRepository
{
    public VideoRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper,
        uow)
    {
    }

    public async Task<Video?> GetByIdOnPlatformAsync(string idOnPlatform, EPlatform platform)
    {
        return AttachIfNotAttached(await Entities
            .Where(v => v.IdOnPlatform == idOnPlatform && v.Platform == platform)
            .ProjectTo<Video?>(Mapper.ConfigurationProvider)
            .SingleOrDefaultAsync());
    }

    public async Task<ICollection<string>> GetAllIdsOnPlatformWithUnarchivedComments(EPlatform platform)
    {
        return await Entities
            .Where(v => v.Platform == platform && v.LastCommentsFetch == null && v.IsAvailable)
            .OrderBy(v => v.AddedToArchiveAt)
            .Select(v => v.IdOnPlatform)
            .ToListAsync();
    }

    public async Task<ICollection<string>> GetAllIdsOnPlatformNotDownloaded(EPlatform platform)
    {
        return await Entities
            .Where(v => v.Platform == platform && v.IsAvailable &&
                        (v.LocalVideoFiles == null || v.LocalVideoFiles.Count == 0)) // NB! Postgres specific probably!
            .OrderBy(v => v.AddedToArchiveAt)
            .Select(v => v.IdOnPlatform)
            .ToListAsync();
    }

    public async Task<VideoWithBasicAuthors> GetByIdWithBasicAuthorsAsync(Guid id)
    {
        return AttachIfNotAttached(await Entities
            .ProjectTo<VideoWithBasicAuthors>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(v => v.Id == id))!;
    }

    public async Task<VideoWithBasicAuthorsAndComments> GetByIdWithBasicAuthorsAndCommentsAsync(Guid id)
    {
        return AttachIfNotAttached(await Entities
            .ProjectTo<VideoWithBasicAuthorsAndComments>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(v => v.Id == id))!;
    }

    public async Task<ICollection<VideoFile>?> GetVideoFilesAsync(Guid videoId)
    {
        return await Entities.Where(v => v.Id == videoId).Select(v => v.LocalVideoFiles).FirstOrDefaultAsync();
    }

    public async Task<ICollection<VideoWithBasicAuthors>> SearchVideosAsync(EPlatform? platform, string? name, string? author, ICollection<Guid> categoryIds, Guid? userId, Guid? userAuthorId, bool accessAllowed)
    {
        IQueryable<App.Domain.Video> query;
        if (name != null)
        {
            query = Entities.FromSql(
                $"SELECT * FROM \"Videos\" c WHERE jsonb_path_exists(c.\"Title\", ('$.* ? (@ like_regex \"(?i)' || {name} || '\")')::jsonpath)");
        } else
        {
            query = Entities;
        }

        query.Include(e => e.VideoAuthors!)
            .ThenInclude(e => e.Author);
        
        if (platform != null)
        {
            query = query.Where(e => e.Platform == platform);
        }

        if (author != null)
        {
            query = query.Where(e => e.VideoAuthors!.Select(a => a.Author!.UserName + a.Author!.DisplayName).Contains(author));
        }

        if (categoryIds.Count > 0)
        {
            query = query.Where(v =>
                DbContext.VideoCategories
                    .Count(vc => vc.VideoId == v.Id && (vc.AssignedById == null || vc.AssignedById == userAuthorId) && categoryIds.Contains(vc.CategoryId)) == categoryIds.Count);
        }

        if (!accessAllowed)
        {
            query = query.WhereUserIsAllowedToAccessVideoOrVideoIsPublic(DbContext, userId);
        }

        return AttachIfNotAttached<ICollection<VideoWithBasicAuthors>, VideoWithBasicAuthors>(
            await query.ProjectTo<VideoWithBasicAuthors>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<VideoWithComments?> GetByIdOnPlatformWithCommentsAsync(string idOnPlatform, EPlatform platform)
    {
        return AttachIfNotAttached(await Entities
            .Where(v => v.Platform == platform && v.IdOnPlatform == idOnPlatform)
            .Include(v => v.Comments)
            .ProjectTo<VideoWithComments?>(Mapper.ConfigurationProvider)
            .SingleOrDefaultAsync());
    }

    public async Task<ICollection<Video>> GetAllNotOfficiallyFetched(EPlatform platform, int? limit = null)
    {
        IQueryable<App.Domain.Video> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable && v.LastSuccessfulFetchOfficial == null)
            .OrderBy(v => v.AddedToArchiveAt);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return AttachIfNotAttached<ICollection<Video>, Video>(
            await query.ProjectTo<Video>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public async Task<ICollection<Video>> GetAllBeforeOfficialApiFetch(EPlatform platform, DateTime cutoff,
        int? limit = null)
    {
        IQueryable<App.Domain.Video> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable &&
                        v.LastFetchOfficial != null && v.LastFetchOfficial < cutoff)
            .OrderBy(v => v.LastFetchOfficial);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return AttachIfNotAttached<ICollection<Video>, Video>(
            await query.ProjectTo<Video>(Mapper.ConfigurationProvider).ToListAsync());
    }

    public Domain.Video Map(BasicVideoData video, Domain.Video mapped)
    {
        return Mapper.Map(video, mapped);
    }
}