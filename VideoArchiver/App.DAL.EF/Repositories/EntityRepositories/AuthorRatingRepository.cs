using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class AuthorRatingRepository : BaseAppEntityRepository<App.Domain.AuthorRating, AuthorRating>,
    IAuthorRatingRepository
{
    public AuthorRatingRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}