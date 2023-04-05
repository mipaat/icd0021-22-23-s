using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class AuthorPubSubRepository : BaseEntityRepository<AuthorPubSub, AbstractAppDbContext>, IAuthorPubSubRepository
{
    public AuthorPubSubRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<AuthorPubSub> DefaultIncludes(DbSet<AuthorPubSub> entities)
    {
        entities.Include(aps => aps.Author);
        return entities;
    }
}