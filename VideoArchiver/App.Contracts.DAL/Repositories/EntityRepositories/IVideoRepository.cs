using App.Domain;
using App.Domain.Enums;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoRepository : IBaseEntityRepository<Video>
{
    public Task<Video?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform);
    public Task<ICollection<string>> GetAllIdsOnPlatformWithUnarchivedComments(Platform platform);
    public Task<Video?> GetByIdOnPlatformWithCommentsAsync(string idOnPlatform, Platform platform);
    public Task<ICollection<Video>> GetAllNotOfficiallyFetched(Platform platform, int? limit = null);
    public Task<ICollection<Video>> GetAllBeforeOfficialApiFetch(Platform platform, DateTime cutoff, int? limit = null);
}