using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class AuthorSubscriptionRepository : BaseEntityRepository<AuthorSubscription, AbstractAppDbContext>,
    IAuthorSubscriptionRepository
{
    public AuthorSubscriptionRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<AuthorSubscription> DefaultIncludes(DbSet<AuthorSubscription> entities)
    {
        entities
            .Include(e => e.Subscriber)
            .Include(e => e.SubscriptionTarget);
        return entities;
    }
}