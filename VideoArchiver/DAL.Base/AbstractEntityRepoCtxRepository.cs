using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base;

public abstract class
    AbstractEntityRepoCtxRepository<TEntity, TDbContext, TRepositoryContext> :
        AbstractEntityRepoCtxRepository<TEntity, Guid, TDbContext, TRepositoryContext>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
{
    protected AbstractEntityRepoCtxRepository(TDbContext dbContext, TRepositoryContext repositoryContext) :
        base(dbContext, repositoryContext)
    {
    }
}

public abstract class
    AbstractEntityRepoCtxRepository<TEntity, TKey, TDbContext, TRepositoryContext> :
        AbstractEntityRepository<TEntity, TKey, TDbContext>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    protected TRepositoryContext RepositoryContext { get; }

    protected AbstractEntityRepoCtxRepository(TDbContext dbContext, TRepositoryContext repositoryContext) :
        base(dbContext)
    {
        RepositoryContext = repositoryContext;
    }
}