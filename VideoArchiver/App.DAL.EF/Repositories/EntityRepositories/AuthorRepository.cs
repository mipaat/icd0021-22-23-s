using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using App.Domain.Enums;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class AuthorRepository : BaseEntityRepository<Author, AbstractAppDbContext>, IAuthorRepository
{
    public AuthorRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Author?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform)
    {
        return await Entities
                    .Where(a => a.IdOnPlatform == idOnPlatform && a.Platform == platform)
                    .SingleOrDefaultAsync();
    }
}