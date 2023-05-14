using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class ApiQuotaUsageRepository : BaseAppEntityRepository<App.Domain.ApiQuotaUsage, ApiQuotaUsage>,
    IApiQuotaUsageRepository
{
    public ApiQuotaUsageRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    public async Task<ApiQuotaUsage?> GetLatestByIdentifier(string identifier)
    {
        return Mapper.Map(await Entities
            .Where(a => a.Identifier == identifier)
            .OrderByDescending(a => a.UsageDate)
            .FirstOrDefaultAsync());
    }
}