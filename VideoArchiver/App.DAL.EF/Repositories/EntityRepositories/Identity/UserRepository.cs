using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using App.Domain.Identity;
using DAL.Base;

namespace DAL.Repositories.EntityRepositories.Identity;

public class UserRepository : BaseEntityRepository<User, AbstractAppDbContext>, IUserRepository
{
    public UserRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}