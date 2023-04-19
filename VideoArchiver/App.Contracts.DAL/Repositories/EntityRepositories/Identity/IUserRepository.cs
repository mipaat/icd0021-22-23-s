using App.Domain.Identity;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories.Identity;

public interface IUserRepository : IBaseEntityRepository<User>
{
}