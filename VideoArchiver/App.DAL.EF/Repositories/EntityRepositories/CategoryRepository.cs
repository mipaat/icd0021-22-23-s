using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class CategoryRepository : BaseEntityRepository<Category, AbstractAppDbContext>, ICategoryRepository
{
    public CategoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<Category> DefaultIncludes(DbSet<Category> entities)
    {
        entities
            .Include(c => c.Creator)
            .Include(c => c.ParentCategory);
        return entities;
    }
}