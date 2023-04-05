using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class AuthorCategoryRepository : BaseEntityRepository<AuthorCategory, AbstractAppDbContext>,
    IAuthorCategoryRepository
{
    public AuthorCategoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<AuthorCategory> DefaultIncludes(DbSet<AuthorCategory> entities)
    {
        entities
            .Include(ac => ac.Author)
            .Include(ac => ac.Category);
        return entities;
    }
}