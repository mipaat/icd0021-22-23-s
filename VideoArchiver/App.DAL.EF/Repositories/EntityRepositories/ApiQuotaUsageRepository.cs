using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class ApiQuotaUsageRepository : BaseEntityRepository<ApiQuotaUsage, AbstractAppDbContext>, IApiQuotaUsageRepository
{
    public ApiQuotaUsageRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ApiQuotaUsage?> GetLatestByIdentifier(string identifier)
    {
        return await Entities
            .Where(a => a.Identifier == identifier)
            .OrderByDescending(a => a.UsageDate)
            .FirstOrDefaultAsync();
    }
}