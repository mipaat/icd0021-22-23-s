using App.Contracts.DAL.Repositories.EntityRepositories.Identity;
using App.Domain;
using App.Domain.Identity;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories.Identity;

public class UserRepository : BaseEntityRepository<User, AbstractAppDbContext>, IUserRepository
{
    public UserRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<Author>> GetAllUserSubAuthors(Guid userId)
    {
        return await DbContext.Authors.Where(a => a.UserId == userId).ToListAsync();
    }

    public async Task<bool> IsUserSubAuthor(Guid authorId, Guid userId)
    {
        return await DbContext.Authors.Where(a => a.Id == authorId && a.UserId == userId).AnyAsync();
    }
}