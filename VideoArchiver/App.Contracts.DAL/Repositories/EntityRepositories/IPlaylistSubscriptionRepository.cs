using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistSubscriptionRepository : IBaseEntityRepository<PlaylistSubscription, App.DAL.DTO.Entities.PlaylistSubscription>
{
}