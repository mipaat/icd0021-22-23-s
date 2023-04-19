using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class GameRepository : BaseEntityRepository<Game, AbstractAppDbContext>, IGameRepository
{
    public GameRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}