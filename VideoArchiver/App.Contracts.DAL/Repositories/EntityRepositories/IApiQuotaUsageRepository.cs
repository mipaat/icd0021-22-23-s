using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IApiQuotaUsageRepository : IBaseEntityRepository<ApiQuotaUsage>
{
    public Task<ApiQuotaUsage?> GetLatestByIdentifier(string identifier);
}