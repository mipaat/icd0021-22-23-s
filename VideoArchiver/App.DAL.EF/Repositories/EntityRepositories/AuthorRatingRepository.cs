using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class AuthorRatingRepository : BaseAppEntityRepository<App.Domain.AuthorRating, AuthorRating>,
    IAuthorRatingRepository
{
    public AuthorRatingRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}