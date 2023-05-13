using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class AuthorCategoryRepository : BaseAppEntityRepository<App.Domain.AuthorCategory, AuthorCategory>,
    IAuthorCategoryRepository
{
    public AuthorCategoryRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}