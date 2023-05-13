using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class ExternalUserTokenRepository : BaseAppEntityRepository<App.Domain.ExternalUserToken, ExternalUserToken>,
    IExternalUserTokenRepository
{
    public ExternalUserTokenRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}