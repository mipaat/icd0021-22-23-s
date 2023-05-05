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
                        // TODO: Else delete entity? How?

                        if (dbValues != null)
                        {
                            entry.OriginalValues.SetValues(dbValues);
                        }
                        // TODO: Else add entity??? Is that even needed? How?
                    }
                }
            }
        }
    }

    public event EventHandler? SavedChanges;

    private readonly Dictionary<Type, HashSet<ConcurrencyConflictResolverUntypedAsync>> _concurrencyConflictResolversAsync =
        new();

    private readonly Dictionary<Type, HashSet<ConcurrencyConflictResolverUntyped>> _concurrencyConflictResolvers = new();

    public void AddConcurrencyConflictResolver<TEntity>(ConcurrencyConflictResolverAsync<TEntity> concurrencyConflictResolver)
    {
        if (!_concurrencyConflictResolversAsync.ContainsKey(typeof(TEntity)))
        {
            _concurrencyConflictResolversAsync[typeof(TEntity)] =
                new HashSet<ConcurrencyConflictResolverUntypedAsync>();
        }

        _concurrencyConflictResolversAsync[typeof(TEntity)]
            .Add(concurrencyConflictResolver.ToUntypedConcurrencyConflictResolver());
    }

    public void AddConcurrencyConflictResolver<TEntity>(ConcurrencyConflictResolver<TEntity> concurrencyConflictResolver)
    {
        if (!_concurrencyConflictResolvers.ContainsKey(typeof(TEntity)))
        {
            _concurrencyConflictResolvers[typeof(TEntity)] = new HashSet<ConcurrencyConflictResolverUntyped>();
        }

        _concurrencyConflictResolvers[typeof(TEntity)].Add(concurrencyConflictResolver.ToUntypedConcurrencyConflictResolver());
    }
}