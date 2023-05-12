using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistRepository : IBaseEntityRepository<Playlist, App.DAL.DTO.Entities.Playlist>
{
    public Task<App.DAL.DTO.Entities.Playlist?> GetByIdOnPlatformAsync(string idOnPlatform, App.DAL.DTO.Enums.Platform platform);
    public Task<ICollection<App.DAL.DTO.Entities.Playlist>> GetAllNotOfficiallyFetched(App.DAL.DTO.Enums.Platform platform, int? limit = null);
    public Task<ICollection<App.DAL.DTO.Entities.Playlist>> GetAllBeforeOfficialApiFetch(App.DAL.DTO.Enums.Platform platform, DateTime cutoff, int? limit = null);
}