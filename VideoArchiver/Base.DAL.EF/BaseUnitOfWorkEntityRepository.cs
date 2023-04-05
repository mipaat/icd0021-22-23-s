using Contracts.DAL;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base;

public class
    BaseUnitOfWorkEntityRepository<TEntity, TDbContext, TUnitOfWork> :
        BaseUnitOfWorkEntityRepository<TEntity, Guid, TDbContext, TUnitOfWork>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
    where TUnitOfWork : IBaseUnitOfWork
{
    public BaseUnitOfWorkEntityRepository(TDbContext dbContext, TUnitOfWork repositoryContext) :
        base(dbContext, repositoryContext)
    {
    }
}

public class
    BaseUnitOfWorkEntityRepository<TEntity, TKey, TDbContext, TUnitOfWork> :
        BaseEntityRepository<TEntity, TKey, TDbContext>,
        IBaseUnitOfWorkEntityRepository<TEntity, TKey, TUnitOfWork>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
    where TUnitOfWork : IBaseUnitOfWork
{
    public TUnitOfWork Uow { get; }

    public BaseUnitOfWorkEntityRepository(TDbContext dbContext, TUnitOfWork repositoryContext) :
        base(dbContext)
    {
        Uow = repositoryContext;
    }
}