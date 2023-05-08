using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using App.Domain.Enums;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoRepository : BaseEntityRepository<Video, AbstractAppDbContext>, IVideoRepository
{
    public VideoRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Video?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform)
    {
        return await Entities
            // .Include(v => v.VideoAuthors!)
            // .ThenInclude(va => va.Author)
            .Where(v => v.IdOnPlatform == idOnPlatform && v.Platform == platform)
            .SingleOrDefaultAsync();
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
        return await Entities
            .Where(v => v.Platform == platform && v.IdOnPlatform == idOnPlatform)
            .Include(v => v.Comments)
            .SingleOrDefaultAsync();
    }

    public async Task<ICollection<Video>> GetAllNotOfficiallyFetched(Platform platform, int? limit = null)
    {
        IQueryable<Video> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable && v.LastSuccessfulFetchOfficial == null)
            .OrderBy(v => v.AddedToArchiveAt);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<ICollection<Video>> GetAllBeforeOfficialApiFetch(Platform platform, DateTime cutoff, int? limit = null)
    {
        IQueryable<Video> query = Entities
            .Where(v => v.Platform == platform && v.IsAvailable &&
                        v.LastFetchOfficial != null && v.LastFetchOfficial < cutoff)
            .OrderBy(v => v.LastFetchOfficial);
        if (limit != null)
        {
            query = query.Take(limit.Value);
        }

        return await query.ToListAsync();
    }
}