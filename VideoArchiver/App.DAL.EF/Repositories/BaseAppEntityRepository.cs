using AutoMapper;
using Base.DAL.EF;
using Domain.Base;

namespace App.DAL.EF.Repositories;

public class BaseAppEntityRepository<TDomainEntity, TEntity> :
    BaseEntityRepository<TDomainEntity, TEntity, AbstractAppDbContext>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TEntity : class, IIdDatabaseEntity<Guid>
{
    public BaseAppEntityRepository(AbstractAppDbContext dbContext, IMapper mapper) :
        base(dbContext, mapper)
    {
    }
}