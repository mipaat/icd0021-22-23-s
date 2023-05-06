using Base.DAL;
using Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base;

public class BaseUnitOfWork<TDbContext> : IBaseUnitOfWork where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;

    protected BaseUnitOfWork(TDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Dispose()
    {
        DbContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync()
    {
        var tryCounter = 0;

        while (true)
        {
            tryCounter++;
            try
            {
                var result = await DbContext.SaveChangesAsync();
                SavedChanges?.Invoke(null, EventArgs.Empty);
                if (SavedChanges != null)
                {
                    foreach (var subscriber in SavedChanges.GetInvocationList())
                    {
                        SavedChanges -= subscriber as EventHandler;
                    }
                }

                return result;
            }
            catch (DbUpdateConcurrencyException e)
            {
                // TODO: What to do if entity has been deleted in database? (Apparently detection would have to be provider dependent)
                // TODO: Constraint violations? Are those covered by DbUpdateConcurrencyException or only DbUpdateException?
                if (tryCounter > 5)
                {
                    throw new FailedToResolveConcurrencyException(e);
                }

                foreach (var entry in e.Entries)
                {
                    var entityType = entry.Metadata.ClrType;
                    var conflictResolversAsync = _concurrencyConflictResolversAsync.GetValueOrDefault(entityType);
                    var conflictResolvers = _concurrencyConflictResolvers.GetValueOrDefault(entityType);
                    object? updatedEntity = null;
                    var resolved = false;
                    if (conflictResolversAsync?.Count > 0 && conflictResolvers?.Count > 0)
                    {
                        var currentEntity = entry.Entity;
                        var dbValues = await entry.GetDatabaseValuesAsync();
                        var dbEntity = dbValues?.ToObject();

                        var dbSet = new ReflectionDbSet(DbContext, entityType);

                        foreach (var conflictResolver in conflictResolversAsync)
                        {
                            try
                            {
                                updatedEntity = await conflictResolver(currentEntity, dbEntity, e);
                                resolved = true;
                                break;
                            }
                            catch (FailedToResolveConcurrencyException)
                            {
                            }
                        }

                        if (!resolved)
                        {
                            foreach (var conflictResolver in conflictResolvers)
                            {
                                try
                                {
                                    updatedEntity = conflictResolver(currentEntity, dbEntity, e);
                                    resolved = true;
                                    break;
                                }
                                catch (FailedToResolveConcurrencyException)
                                {
                                }
                            }
                        }

                        if (!resolved)
                        {
                            throw new FailedToResolveConcurrencyException(e);
                        }

                        if (updatedEntity != null)
                        {
                            entry.CurrentValues.SetValues(updatedEntity);
                        }
                        else
                        {
                            dbSet.Remove(entry.Entity);
                        }

                        if (dbValues != null)
                        {
                            entry.OriginalValues.SetValues(dbValues);
                        }
                        else if (updatedEntity != null)
                        {
                            dbSet.Add(entry.Entity);
                        }
                    }
                }
            }
        }
    }

    public event EventHandler? SavedChanges;

    private readonly Dictionary<Type, List<ConcurrencyConflictResolverUntypedAsync>>
        _concurrencyConflictResolversAsync =
            new();

    private readonly Dictionary<Type, List<ConcurrencyConflictResolverUntyped>> _concurrencyConflictResolvers = new();

    public void AddConcurrencyConflictResolver<TEntity>(
        ConcurrencyConflictResolverAsync<TEntity> concurrencyConflictResolver)
    {
        if (!_concurrencyConflictResolversAsync.ContainsKey(typeof(TEntity)))
        {
            _concurrencyConflictResolversAsync[typeof(TEntity)] =
                new List<ConcurrencyConflictResolverUntypedAsync>();
        }

        _concurrencyConflictResolversAsync[typeof(TEntity)]
            .Insert(0, concurrencyConflictResolver.ToUntypedConcurrencyConflictResolver());
    }

    public void AddConcurrencyConflictResolver<TEntity>(
        ConcurrencyConflictResolver<TEntity> concurrencyConflictResolver)
    {
        if (!_concurrencyConflictResolvers.ContainsKey(typeof(TEntity)))
        {
            _concurrencyConflictResolvers[typeof(TEntity)] = new List<ConcurrencyConflictResolverUntyped>();
        }

        _concurrencyConflictResolvers[typeof(TEntity)]
            .Insert(0, concurrencyConflictResolver.ToUntypedConcurrencyConflictResolver());
    }
}

internal class ReflectionDbSet
{
    private readonly Type _entityType;
    private readonly DbContext _dbContext;
    private object? _dbSetInstance;

    private object DbSetInstance
    {
        get
        {
            return _dbSetInstance ??=
                typeof(DbContext)
                    .GetMethod("Set")?
                    .MakeGenericMethod(_entityType)
                    .Invoke(_dbContext, null)
                ?? throw new NullReferenceException(
                    $"Failed to construct generic DbSet for type {_entityType} using reflection!");
        }
    }

    public ReflectionDbSet(DbContext dbContext, Type entityType)
    {
        _entityType = entityType;
        _dbContext = dbContext;
    }

    public void Remove(object entity)
    {
        var removeMethod = DbSetInstance.GetType().GetMethod("Remove");
        if (removeMethod == null)
        {
            throw new NullReferenceException(
                $"Failed to construct generic DbSet remove method for type {_entityType} using reflection!");
        }

        removeMethod.Invoke(_dbSetInstance, new[] { entity });
    }

    public void Add(object entity)
    {
        var addMethod = DbSetInstance.GetType().GetMethod("Add");
        if (addMethod == null)
        {
            throw new NullReferenceException(
                $"Failed to construct generic DbSet add method for type {_entityType} using reflection!");
        }

        addMethod.Invoke(_dbSetInstance, new[] { entity });
    }
}