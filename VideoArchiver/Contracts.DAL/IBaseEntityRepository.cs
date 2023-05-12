using System.Linq.Expressions;
using Domain.Base;

namespace Contracts.DAL;

public interface IBaseEntityRepository<TDomainEntity, TEntity, TKey>
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
    where TEntity : IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public Task<TEntity?> GetByIdAsync(TKey id);
    public Task<ICollection<TEntity>> GetAllAsync(params Expression<Func<TDomainEntity, bool>>[] filters);
    public TEntity Add(TEntity entity);
    public void Remove(TEntity entity);
    public Task RemoveAsync(TKey id);
    public void Update(TEntity entity);

    public Task<bool> ExistsAsync(TKey id);
}

public interface IBaseEntityRepository<TDomainEntity, TEntity> : IBaseEntityRepository<TDomainEntity, TEntity, Guid>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TEntity : IIdDatabaseEntity<Guid>
{
}