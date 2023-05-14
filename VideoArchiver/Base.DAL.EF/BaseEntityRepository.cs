using System.Linq.Expressions;
using Contracts.DAL;
using Contracts.Mapping;
using Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TDomainEntity, TEntity, TKey, TDbContext, TMapper, TUow> :
    IBaseEntityRepository<TDomainEntity, TEntity, TKey>
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TMapper : IMapper<TDomainEntity, TEntity>
    where TUow : IBaseUnitOfWork
{
    public TDbContext DbContext { get; }
    public readonly TMapper Mapper;

    public TUow Uow { get; }

    public BaseEntityRepository(TDbContext dbContext, TMapper mapper, TUow uow)
    {
        DbContext = dbContext;
        Mapper = mapper;
        Uow = uow;
    }

    protected DbSet<TDomainEntity> Entities =>
        DbContext
            .GetType()
            .GetProperties()
            .FirstOrDefault(pi => pi.PropertyType == typeof(DbSet<TDomainEntity>))
            ?.GetValue(DbContext) as DbSet<TDomainEntity> ??
        throw new ApplicationException(
            $"Failed to fetch DbSet for Entity type {typeof(TDomainEntity)} from {typeof(DbContext)}");

    protected virtual TDomainEntity AfterMap(TEntity entity, TDomainEntity mapped)
    {
        return mapped;
    }

    protected virtual Func<TQueryable, TQueryable>? IncludeDefaultsFunc<TQueryable>()
        where TQueryable : IQueryable<TDomainEntity> => null;

    protected TQueryable IncludeDefaults<TQueryable>(TQueryable queryable)
        where TQueryable : IQueryable<TDomainEntity>
    {
        var includeFunc = IncludeDefaultsFunc<TQueryable>();
        if (includeFunc == null) return queryable;
        return includeFunc(queryable);
    }

    protected IQueryable<TDomainEntity> EntitiesWithDefaults => IncludeDefaults(Entities);

    public TDomainEntity Map(TEntity entity)
    {
        return AfterMap(entity, Mapper.Map(entity)!);
    }

    public TDomainEntity Map(TEntity entity, TDomainEntity domainEntity)
    {
        return AfterMap(entity, Mapper.Map(entity, domainEntity));
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        var includeDefaultsFunc = IncludeDefaultsFunc<DbSet<TDomainEntity>>();
        return Mapper.Map(includeDefaultsFunc != null
            ? await includeDefaultsFunc(Entities).FirstOrDefaultAsync(e => e.Id.Equals(id))
            : await Entities.FindAsync(id));
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
        var result = IncludeDefaults(GetAll(filters));
        return (await result.ToListAsync()).Select(e => Mapper.Map(e)!).ToList();
    }

    public TEntity Add(TEntity entity)
    {
        var trackedEntity = GetTrackedEntity(entity);
        if (trackedEntity != null)
        {
            if (DbContext.ChangeTracker.AutoDetectChangesEnabled)
            {
                Map(entity, trackedEntity);
            }
            else
            {
                Entities.Update(Map(entity, trackedEntity));
            }
        }
        else
        {
            Entities.Add(Map(entity));
        }

        return entity;
    }

    public void Remove(TEntity entity)
    {
        Remove(GetTrackedEntity(entity) ??
               Mapper.Map(entity)!); // TODO: Should this throw if entity is not tracked?
    }

    private void Remove(TDomainEntity entity)
    {
        Entities.Remove(entity);
    }

    public async Task RemoveAsync(TKey id)
    {
        Remove(await Entities.FindAsync(id) ??
               throw new ApplicationException($"Failed to delete entity with ID {id} - entity not found!"));
    }

    public virtual void Update(TEntity entity)
    {
        var trackedEntity = GetTrackedEntity(entity);
        if (trackedEntity != null)
        {
            if (DbContext.ChangeTracker.AutoDetectChangesEnabled)
            {
                Map(entity, trackedEntity);
            }
            else
            {
                Entities.Update(Map(entity, trackedEntity));
            }
        }
        else
        {
            Entities.Update(Map(entity));
        }
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await Entities.AnyAsync(e => e.Id.Equals(id));
    }

    public TDomainEntity? GetTrackedEntity(TEntity entity) =>
        GetTrackedEntity(entity.Id);

    public TDomainEntity? GetTrackedEntity(TKey id) => DbContext.GetTrackedEntity<TDomainEntity, TKey>(id);
}

public class BaseEntityRepository<TDomainEntity, TEntity, TDbContext, TMapper, TUow> :
    BaseEntityRepository<TDomainEntity, TEntity, Guid, TDbContext, TMapper, TUow>,
    IBaseEntityRepository<TDomainEntity, TEntity>
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDbContext : DbContext
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
    where TMapper : IMapper<TDomainEntity, TEntity>
    where TUow : IBaseUnitOfWork
{
    public BaseEntityRepository(TDbContext dbContext, TMapper mapper, TUow uow) : base(dbContext, mapper, uow)
    {
    }
}