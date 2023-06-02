using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IPlaylistSubscriptionRepository : IBaseEntityRepository<PlaylistSubscription, App.DAL.DTO.Entities.Playlists.PlaylistSubscription>
{
}