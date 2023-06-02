using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class AuthorRatingRepository : BaseAppEntityRepository<App.Domain.AuthorRating, AuthorRating>,
    IAuthorRatingRepository
{
    public AuthorRatingRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }
}