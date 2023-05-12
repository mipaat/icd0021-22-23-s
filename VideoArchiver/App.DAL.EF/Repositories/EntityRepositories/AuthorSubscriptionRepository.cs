using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class AuthorSubscriptionRepository : BaseAppEntityRepository<App.Domain.AuthorSubscription, AuthorSubscription>,
    IAuthorSubscriptionRepository
{
    public AuthorSubscriptionRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}