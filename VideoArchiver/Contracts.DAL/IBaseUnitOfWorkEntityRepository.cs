using Domain.Base;

namespace Contracts.DAL;

public interface IBaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TUnitOfWork> : IBaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, Guid, TUnitOfWork>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TUnitOfWork : IBaseUnitOfWork
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
{
}

public interface IBaseUnitOfWorkEntityRepository<TDomainEntity, TEntity, TKey, TUnitOfWork> : IBaseEntityRepository<TDomainEntity, TEntity, TKey>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TUnitOfWork : IBaseUnitOfWork
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
{
    public TUnitOfWork Uow { get; }
}