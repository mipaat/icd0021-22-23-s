using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class AuthorHistoryRepository : BaseEntityRepository<AuthorHistory, AbstractAppDbContext>, IAuthorHistoryRepository
{
    public AuthorHistoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<AuthorHistory> DefaultIncludes(DbSet<AuthorHistory> entities)
    {
        entities.Include(ah => ah.Author);
        return entities;
    }
}