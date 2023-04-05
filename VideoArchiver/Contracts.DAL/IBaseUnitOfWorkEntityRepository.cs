using Domain.Base;

namespace Contracts.DAL;

public interface IBaseUnitOfWorkEntityRepository<TEntity, TUnitOfWork> : IBaseUnitOfWorkEntityRepository<TEntity, Guid, TUnitOfWork>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TUnitOfWork : IBaseUnitOfWork
{
}

public interface IBaseUnitOfWorkEntityRepository<TEntity, TKey, TUnitOfWork> : IBaseEntityRepository<TEntity, TKey>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TUnitOfWork : IBaseUnitOfWork
{
    public TUnitOfWork Uow { get; }
}