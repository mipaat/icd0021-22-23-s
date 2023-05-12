using Contracts.DAL;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class
    BaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TDbContext, TUnitOfWork, TAutoMapperWrapper> :
        BaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, Guid, TDbContext, TUnitOfWork, TAutoMapperWrapper>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
    where TUnitOfWork : IBaseUnitOfWork
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TAutoMapperWrapper : ITrackingAutoMapperWrapper
{
    public BaseUnitOfWorkEntityRepository(TDbContext dbContext, TUnitOfWork repositoryContext,
        TAutoMapperWrapper mapper) :
        base(dbContext, repositoryContext, mapper)
    {
    }
}

public class
    BaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TKey, TDbContext, TUnitOfWork, TAutoMapperWrapper> :
        BaseEntityRepository<TDomainEntity, TEntity, TKey, TDbContext, TAutoMapperWrapper>,
        IBaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TKey, TUnitOfWork>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
    where TUnitOfWork : IBaseUnitOfWork
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
    where TAutoMapperWrapper : ITrackingAutoMapperWrapper
{
    public TUnitOfWork Uow { get; }

    public BaseUnitOfWorkEntityRepository(TDbContext dbContext, TUnitOfWork repositoryContext,
        TAutoMapperWrapper mapper) :
        base(dbContext, mapper)
    {
        Uow = repositoryContext;
    }
}