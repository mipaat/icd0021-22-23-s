using Base.DAL.EF;
using Contracts.DAL;
using Domain.Base;

namespace DAL.Repositories;

public class BaseAppEntityRepository<TDomainEntity, TEntity> :
    BaseEntityRepository<TDomainEntity, TEntity, AbstractAppDbContext, ITrackingAutoMapperWrapper>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TEntity : class, IIdDatabaseEntity<Guid>
{
    public BaseAppEntityRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) :
        base(dbContext, mapper)
    {
    }
}