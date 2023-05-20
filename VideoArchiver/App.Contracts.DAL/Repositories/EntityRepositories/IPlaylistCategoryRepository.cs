using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistCategoryRepository : IBaseEntityRepository<PlaylistCategory, App.DAL.DTO.Entities.Playlists.PlaylistCategory>
{
    public Task<ICollection<Guid>> GetAllCategoryIdsAsync(Guid playlistId, Guid? authorId);
}