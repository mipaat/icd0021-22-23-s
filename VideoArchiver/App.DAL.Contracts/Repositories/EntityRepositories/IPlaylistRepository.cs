using App.Common.Enums;
using App.DAL.DTO.Entities.Playlists;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IPlaylistRepository : IBaseEntityRepository<App.Domain.Playlist, Playlist>
{
    public Task<Playlist?> GetByIdOnPlatformAsync(string idOnPlatform, EPlatform platform);
    public Task<ICollection<Playlist>> GetAllNotOfficiallyFetched(EPlatform platform, int? limit = null);
    public Task<ICollection<Playlist>> GetAllBeforeOfficialApiFetch(EPlatform platform, DateTime cutoff, int? limit = null);

    public Task<ICollection<PlaylistWithBasicVideoData>> GetAllWithContentsUpdatedBefore(EPlatform platform,
        DateTime cutoff, int? limit = null);

    public void UpdateContents(PlaylistWithBasicVideoData playlist);
}