using App.DAL.DTO.Entities.Identity;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories.Identity;

public interface IUserRepository : IBaseEntityRepository<App.Domain.Identity.User, User>
{
}