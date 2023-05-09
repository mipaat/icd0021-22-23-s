using App.Domain;
using App.Domain.Identity;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories.Identity;

public interface IUserRepository : IBaseEntityRepository<User>
{
    public Task<ICollection<Author>> GetAllUserSubAuthors(Guid userId);

    public Task<bool> IsUserSubAuthor(Guid authorId, Guid userId);
}