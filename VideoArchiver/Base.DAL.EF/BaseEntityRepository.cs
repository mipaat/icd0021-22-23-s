using System.Linq.Expressions;
using AutoMapper;
using Contracts.DAL;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TDomainEntity, TEntity, TKey, TDbContext> :
    IBaseEntityRepository<TDomainEntity, TEntity, TKey>
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
    where TEntity : class, IIdDatabaseEntity<TKey>
{
    public TDbContext DbContext { get; }
    public readonly IMapper<TDomainEntity, TEntity> Mapper;

    public BaseEntityRepository(TDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Mapper = new BaseDbSetTrackingMapper<TDomainEntity, TEntity, TKey>(mapper, Entities);
    }

    protected DbSet<TDomainEntity> Entities =>
        DbContext
            .GetType()
            .GetProperties()
            .FirstOrDefault(pi => pi.PropertyType == typeof(DbSet<TDomainEntity>))
            ?.GetValue(DbContext) as DbSet<TDomainEntity> ??
        throw new ApplicationException(
            $"Failed to fetch DbSet for Entity type {typeof(TDomainEntity)} from {typeof(DbContext)}");

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return Mapper.Map(await Entities.FindAsync(id));
    }

    protected IQueryable<TDomainEntity> GetAll(params Expression<Func<TDomainEntity, bool>>[] filters)
    {
        IQueryable<TDomainEntity> result = Entities;

        foreach (var filter in filters)
        {
            result = result.Where(filter);
        }

        return result;
    }

    public async Task<ICollection<TEntity>> GetAllAsync(params Expression<Func<TDomainEntity, bool>>[] filters)
    {
        var result = GetAll(filters);
        return (await result.ToListAsync()).Select(e => Mapper.Map(e)!).ToList();
    }

    public TEntity Add(TEntity entity)
    {
        Entities.Add(Mapper.Map(entity)!);
        return entity;
    }

    public void Remove(TEntity entity)
    {
        Remove(Mapper.Map(entity)!);
    }

    private void Remove(TDomainEntity entity)
    {
        Entities.Remove(entity);
    }

    public async Task RemoveAsync(TKey id)
    {
        Remove(await GetByIdAsync(id) ??
               throw new ApplicationException($"Failed to delete entity with ID {id} - entity not found!"));
    }

    public void Update(TEntity entity)
    {
        var trackedEntity = Entities.Local.FirstOrDefault(e => e.Id.Equals(entity.Id));
        if (trackedEntity != null)
        {
            Mapper.Map(entity, trackedEntity);
        }
        else
        {
            Entities.Update(Mapper.Map(entity)!);
        }
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await Entities.AnyAsync(e => e.Id.Equals(id));
    }
}

public class BaseEntityRepository<TDomainEntity, TEntity, TDbContext> :
    BaseEntityRepository<TDomainEntity, TEntity, Guid, TDbContext>,
    IBaseEntityRepository<TDomainEntity, TEntity>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
{
    public BaseEntityRepository(TDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}