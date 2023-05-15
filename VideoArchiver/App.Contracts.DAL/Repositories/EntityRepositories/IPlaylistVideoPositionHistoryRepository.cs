using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistVideoPositionHistoryRepository : IBaseEntityRepository<PlaylistVideoPositionHistory, App.DAL.DTO.Entities.Playlists.PlaylistVideoPositionHistory>
{
}