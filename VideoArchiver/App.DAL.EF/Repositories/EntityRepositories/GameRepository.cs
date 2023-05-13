using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class GameRepository : BaseAppEntityRepository<App.Domain.Game, Game>, IGameRepository
{
    public GameRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}