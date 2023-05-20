using App.DAL.DTO.Entities.Playlists;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistVideoRepository : IBaseEntityRepository<App.Domain.PlaylistVideo, PlaylistVideo>
{
    public void Add(BasicPlaylistVideo playlistVideo, Guid playlistId);
    public void Update(BasicPlaylistVideo playlistVideo);
}