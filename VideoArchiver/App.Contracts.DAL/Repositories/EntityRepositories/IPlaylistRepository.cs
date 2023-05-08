using App.Domain;
using App.Domain.Enums;
using Contracts.DAL;
using Domain;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistRepository : IBaseEntityRepository<Playlist>
{
    public Task<Playlist?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform);
    public Task<ICollection<Playlist>> GetAllNotOfficiallyFetched(Platform platform, int? limit = null);
    public Task<ICollection<Playlist>> GetAllBeforeOfficialApiFetch(Platform platform, DateTime cutoff, int? limit = null);
}