using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IPlaylistRatingRepository : IBaseEntityRepository<PlaylistRating, App.DAL.DTO.Entities.Playlists.PlaylistRating>
{
}