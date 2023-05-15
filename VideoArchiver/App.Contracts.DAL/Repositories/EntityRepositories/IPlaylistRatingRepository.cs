using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistRatingRepository : IBaseEntityRepository<PlaylistRating, App.DAL.DTO.Entities.Playlists.PlaylistRating>
{
}