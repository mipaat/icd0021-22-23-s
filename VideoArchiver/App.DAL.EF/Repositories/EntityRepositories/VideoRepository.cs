using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.Common.Enums;
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
            .Where(v => v.Platform == platform && v.IsAvailable && (v.LocalVideoFiles == null || v.LocalVideoFiles.Count == 0)) // NB! Postgres specific probably!
            .OrderBy(v => v.AddedToArchiveAt)
            .Select(v => v.IdOnPlatform)
            .ToListAsync();
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