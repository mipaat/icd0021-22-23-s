using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IApiQuotaUsageRepository : IBaseEntityRepository<ApiQuotaUsage, App.DAL.DTO.Entities.ApiQuotaUsage>
{
    public Task<App.DAL.DTO.Entities.ApiQuotaUsage?> GetLatestByIdentifier(string identifier);
}