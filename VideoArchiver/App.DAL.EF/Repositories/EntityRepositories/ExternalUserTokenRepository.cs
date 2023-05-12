using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class ExternalUserTokenRepository : BaseAppEntityRepository<App.Domain.ExternalUserToken, ExternalUserToken>,
    IExternalUserTokenRepository
{
    public ExternalUserTokenRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}