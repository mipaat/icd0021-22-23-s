using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoGameRepository : IBaseEntityRepository<VideoGame, App.DAL.DTO.Entities.VideoGame>
{
}