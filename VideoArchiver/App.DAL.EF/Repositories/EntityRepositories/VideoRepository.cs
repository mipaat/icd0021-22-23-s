using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoRepository : BaseAppEntityRepository<App.Domain.Video, Video>, IVideoRepository
{
    public VideoRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Video?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform)
    {
        return Mapper.Map(await Entities
            .Where(v => v.IdOnPlatform == idOnPlatform && v.Platform == platform)
            .SingleOrDefaultAsync());
    }

    public async Task<ICollection<string>> GetAllIdsOnPlatformWithUnarchivedComments(Platform platform)
    {
        return await Entities
            .Where(v => v.Platform == platform && v.LastCommentsFetch == null && v.IsAvailable)
            .OrderBy(v => v.AddedToArchiveAt)
            .Select(v => v.IdOnPlatform)
            .ToListAsync();
    }

    public async Task<Video?> GetByIdOnPlatformWithCommentsAsync(string idOnPlatform, Platform platform)
    {
        return Mapper.Map(await Entities
            .Where(v => v.Platform == platform && v.IdOnPlatform == idOnPlatform)
            .Include(v => v.Comments)
            .SingleOrDefaultAsync());
    }

    public async Task<ICollection<Video>> GetAllNotOfficiallyFetched(Platform platform, int? limit = null)
    {
        IQueryable<App.Domain.Video> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable && v.LastSuccessfulFetchOfficial == null)
            .OrderBy(v => v.AddedToArchiveAt);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return (await query.ToListAsync()).Select(e => Mapper.Map(e)!).ToList();
    }

    public async Task<ICollection<Video>> GetAllBeforeOfficialApiFetch(Platform platform, DateTime cutoff,
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

        return (await query.ToListAsync()).Select(e => Mapper.Map(e)!).ToList();
    }
}