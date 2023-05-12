using AutoMapper;
using Contracts.DAL;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class
    BaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TDbContext, TUnitOfWork> :
        BaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, Guid, TDbContext, TUnitOfWork>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
    where TUnitOfWork : IBaseUnitOfWork
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
{
    public BaseUnitOfWorkEntityRepository(TDbContext dbContext, TUnitOfWork repositoryContext, IMapper mapper) :
        base(dbContext, repositoryContext, mapper)
    {
    }
}

public class
    BaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TKey, TDbContext, TUnitOfWork> :
        BaseEntityRepository<TDomainEntity, TEntity, TKey, TDbContext>,
        IBaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TKey, TUnitOfWork>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
    where TUnitOfWork : IBaseUnitOfWork
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
{
    public TUnitOfWork Uow { get; }

    public BaseUnitOfWorkEntityRepository(TDbContext dbContext, TUnitOfWork repositoryContext, IMapper mapper) :
        base(dbContext, mapper)
    {
        Uow = repositoryContext;
    }
}