using System.Linq.Expressions;
using Contracts.DAL;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base;

public class BaseEntityRepository<TEntity, TKey, TDbContext> : IBaseEntityRepository<TEntity, TKey>
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    public TDbContext DbContext { get; }

    public BaseEntityRepository(TDbContext dbContext)
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

    protected virtual DbSet<TEntity> DefaultIncludes(DbSet<TEntity> entities)
    {
        return entities;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await DefaultIncludes(Entities).FindAsync(id);
    }

    protected IQueryable<TEntity> GetAll(params Expression<Func<TEntity, bool>>[] filters)
    {
        return GetAll(DefaultIncludes, filters);
    }

    protected IQueryable<TEntity> GetAll(Func<DbSet<TEntity>, DbSet<TEntity>> includes, params Expression<Func<TEntity, bool>>[] filters)
    {
        IQueryable<TEntity> result = includes(Entities);

        foreach (var filter in filters)
        {
            result = result.Where(filter);
        }
        
        return result;
    }

    public async Task<ICollection<TEntity>> GetAllAsync(params Expression<Func<TEntity, bool>>[] filters)
    {
        var result = GetAll(filters);
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

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await Entities.AnyAsync(e => e.Id.Equals(id));
    }
}

public class BaseEntityRepository<TEntity, TDbContext> :
    BaseEntityRepository<TEntity, Guid, TDbContext>, IBaseEntityRepository<TEntity>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext) : base(dbContext)
    {
    }
}