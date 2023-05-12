using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories.Identity;

public interface IUserRepository : IBaseEntityRepository<App.Domain.Identity.User, App.Domain.Identity.User>
{
}