using App.Domain;
using Contracts.DAL;
using Domain;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IGameRepository : IBaseEntityRepository<Game, App.DAL.DTO.Entities.Game>
{
}