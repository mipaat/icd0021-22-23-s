using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IPlaylistHistoryRepository : IBaseEntityRepository<PlaylistHistory, App.DAL.DTO.Entities.Playlists.PlaylistHistory>
{
}