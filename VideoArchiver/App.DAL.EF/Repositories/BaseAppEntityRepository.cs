using App.Contracts.DAL;
using AutoMapper;
using Base.DAL.EF;
using Base.Mapping;
using Contracts.Mapping;
using Domain.Base;

namespace App.DAL.EF.Repositories;

public class BaseAppEntityRepository<TDomainEntity, TEntity, TMapper> :
    BaseEntityRepository<TDomainEntity, TEntity, AbstractAppDbContext, TMapper, IAppUnitOfWork>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TMapper : IMapper<TDomainEntity, TEntity>
{
    public BaseAppEntityRepository(AbstractAppDbContext dbContext, TMapper mapper, IAppUnitOfWork uow) :
        base(dbContext, mapper, uow)
    {
    }
}

public class BaseAppEntityRepository<TDomainEntity, TEntity> :
    BaseEntityRepository<TDomainEntity, TEntity, AbstractAppDbContext, BaseMapper<TDomainEntity, TEntity>, IAppUnitOfWork>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TEntity : class, IIdDatabaseEntity<Guid>
{
    public BaseAppEntityRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) :
        base(dbContext, new BaseMapper<TDomainEntity, TEntity>(mapper), uow)
    {
    }
}