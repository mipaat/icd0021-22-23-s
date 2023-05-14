using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoRepository : IBaseEntityRepository<Video, App.DAL.DTO.Entities.Video>
{
    public Task<App.DAL.DTO.Entities.Video?> GetByIdOnPlatformAsync(string idOnPlatform, App.DAL.DTO.Enums.Platform platform);
    public Task<ICollection<string>> GetAllIdsOnPlatformWithUnarchivedComments(App.DAL.DTO.Enums.Platform platform);
    public Task<App.DAL.DTO.Entities.VideoWithComments?> GetByIdOnPlatformWithCommentsAsync(string idOnPlatform, App.DAL.DTO.Enums.Platform platform);
    public Task<ICollection<App.DAL.DTO.Entities.Video>> GetAllNotOfficiallyFetched(App.DAL.DTO.Enums.Platform platform, int? limit = null);
    public Task<ICollection<App.DAL.DTO.Entities.Video>> GetAllBeforeOfficialApiFetch(App.DAL.DTO.Enums.Platform platform, DateTime cutoff, int? limit = null);
}