using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class AuthorCategoryRepository : BaseAppEntityRepository<App.Domain.AuthorCategory, AuthorCategory>,
    IAuthorCategoryRepository
{
    public AuthorCategoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}