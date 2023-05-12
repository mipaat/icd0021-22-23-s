using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IPlaylistVideoRepository : IBaseEntityRepository<PlaylistVideo, App.DAL.DTO.Entities.PlaylistVideo>
{
}