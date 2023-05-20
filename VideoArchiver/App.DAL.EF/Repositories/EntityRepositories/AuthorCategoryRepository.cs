using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class AuthorCategoryRepository : BaseAppEntityRepository<App.Domain.AuthorCategory, AuthorCategory>,
    IAuthorCategoryRepository
{
    public AuthorCategoryRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    public async Task<ICollection<Guid>> GetAllCategoryIdsAsync(Guid authorId, Guid? assignedByAuthorId)
    {
        return await Entities.Where(e => e.AuthorId == authorId && e.AssignedById == assignedByAuthorId)
            .Select(e => e.CategoryId)
            .ToListAsync();
    }
}