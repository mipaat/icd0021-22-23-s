using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IPlaylistVideoPositionHistoryRepository : IBaseEntityRepository<PlaylistVideoPositionHistory, App.DAL.DTO.Entities.Playlists.PlaylistVideoPositionHistory>
{
}