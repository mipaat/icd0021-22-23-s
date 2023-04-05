using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class CategoryRepository : BaseEntityRepository<Category, AbstractAppDbContext>, ICategoryRepository
{
    public CategoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}