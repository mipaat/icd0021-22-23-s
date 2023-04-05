using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class GameRepository : BaseEntityRepository<Game, AbstractAppDbContext>, IGameRepository
{
    public GameRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}