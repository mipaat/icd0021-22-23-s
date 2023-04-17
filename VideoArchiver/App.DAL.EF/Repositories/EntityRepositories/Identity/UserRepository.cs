using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using DAL.Base;
using Domain.Identity;

namespace DAL.Repositories.EntityRepositories.Identity;

public class UserRepository : BaseEntityRepository<User, AbstractAppDbContext>, IUserRepository
{
    public UserRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}