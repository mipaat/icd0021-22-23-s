using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories.Identity;

public class UserRepository : BaseAppEntityRepository<App.Domain.Identity.User, App.Domain.Identity.User>, IUserRepository
{
    public UserRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}