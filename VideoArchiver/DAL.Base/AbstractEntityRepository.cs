using System.Linq.Expressions;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base;

public abstract class AbstractEntityRepository<TEntity, TKey, TDbContext>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    public TDbContext DbContext { get; }

    protected AbstractEntityRepository(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public virtual DbSet<TEntity> Entities =>
        DbContext
            .GetType()
            .GetProperties()
            .FirstOrDefault(pi => pi.PropertyType == typeof(DbSet<TEntity>))
            ?.GetValue(DbContext) as DbSet<TEntity> ??
        throw new ApplicationException(
            $"Failed to fetch DbSet for Entity type {typeof(TEntity)} from {typeof(DbContext)}");

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await Entities.FindAsync(id);
    }

    public async Task<ICollection<TEntity>> GetAllAsync(params Expression<Func<TEntity, bool>>[] filters)
    {
        IQueryable<TEntity> result = Entities;
        foreach (var filter in filters)
        {
            result = result.Where(filter);
        }

        return await result.ToListAsync();
    }

    public void Add(TEntity entity)
    {
        Entities.Add(entity);
    }

    public void Remove(TEntity entity)
    {
        Entities.Remove(entity);
    }

    public async Task RemoveAsync(TKey id)
    {
        Entities.Remove(await GetByIdAsync(id) ??
                        throw new ApplicationException($"Failed to delete entity with ID {id} - entity not found!"));
    }

    public void Update(TEntity entity)
    {
        Entities.Update(entity);
    }
}

public abstract class AbstractEntityRepository<TEntity, TDbContext> :
    AbstractEntityRepository<TEntity, Guid, TDbContext>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
{
    protected AbstractEntityRepository(TDbContext dbContext) : base(dbContext)
    {
    }
}