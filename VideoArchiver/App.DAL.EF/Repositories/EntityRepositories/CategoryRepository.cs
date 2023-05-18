using App.Common.Enums;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class CategoryRepository : BaseAppEntityRepository<App.Domain.Category, Category>, ICategoryRepository
{
    public CategoryRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    public async Task<ICollection<Category>> GetAllByPlatformAsync(EPlatform platform, IEnumerable<string>? idsOnPlatform = null)
    {
        var query = Entities.Where(e => e.Platform == platform);
        if (idsOnPlatform != null)
        {
            query = query.Where(e => idsOnPlatform.Contains(e.IdOnPlatform));
        }

        return await query.ProjectTo<Category>(Mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<Category?> GetByNameAsync(EPlatform platform, string name)
    {
        return await Entities.FromSql(
                $"SELECT * FROM \"Categories\" c WHERE jsonb_path_exists(c.\"Name\", ('$.* ? (@ like_regex \"(?i)' || {name} || '\")')::jsonpath)")
            .Where(e => e.Platform == platform)
            .ProjectTo<Category>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}