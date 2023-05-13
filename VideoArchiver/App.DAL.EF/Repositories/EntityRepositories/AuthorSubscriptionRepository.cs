using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class AuthorSubscriptionRepository : BaseAppEntityRepository<App.Domain.AuthorSubscription, AuthorSubscription>,
    IAuthorSubscriptionRepository
{
    public AuthorSubscriptionRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}