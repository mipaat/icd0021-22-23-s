using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class AuthorPubSubRepository : BaseAppEntityRepository<App.Domain.AuthorPubSub, AuthorPubSub>,
    IAuthorPubSubRepository
{
    public AuthorPubSubRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}