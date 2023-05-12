using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories.Identity;

public class UserRepository : BaseAppEntityRepository<App.Domain.Identity.User, App.Domain.Identity.User>, IUserRepository
{
    public UserRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}