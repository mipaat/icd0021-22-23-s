using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class AuthorPubSubRepository : BaseAppEntityRepository<App.Domain.AuthorPubSub, AuthorPubSub>,
    IAuthorPubSubRepository
{
    public AuthorPubSubRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }
}