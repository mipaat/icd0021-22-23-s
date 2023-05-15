using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistHistoryRepository : IBaseEntityRepository<PlaylistHistory, App.DAL.DTO.Entities.Playlists.PlaylistHistory>
{
}