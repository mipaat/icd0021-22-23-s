using System.Linq.Expressions;
using Domain.Base;

namespace Contracts.DAL;

public interface IBaseEntityRepository<TEntity, TKey>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public Task<TEntity?> GetByIdAsync(TKey id);
    public Task<ICollection<TEntity>> GetAllAsync(params Expression<Func<TEntity, bool>>[] filters);
    public void Add(TEntity entity);
    public void Remove(TEntity entity);
    public Task RemoveAsync(TKey id);
    public void Update(TEntity entity);

    public Task<bool> ExistsAsync(TKey id);
}

public interface IBaseEntityRepository<TEntity> : IBaseEntityRepository<TEntity, Guid>
    where TEntity : class, IIdDatabaseEntity<Guid>
{
}