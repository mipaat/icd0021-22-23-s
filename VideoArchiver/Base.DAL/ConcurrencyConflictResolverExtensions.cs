using Contracts.DAL;

namespace Base.DAL;

public static class ConcurrencyConflictResolverExtensions
{
    public static ConcurrencyConflictResolverUntypedAsync ToUntypedConcurrencyConflictResolver<TEntity>(
        this ConcurrencyConflictResolverAsync<TEntity> concurrencyConflictResolver)
    {
        return async (oCurrent, oDb, ex) =>
        {
            if (oCurrent is TEntity eCurrent && oDb is TEntity eDb)
            {
                return await concurrencyConflictResolver(eCurrent, eDb, ex);
            }

            throw new FailedToResolveConcurrencyException(ex);
        };
    }

    public static ConcurrencyConflictResolverUntyped ToUntypedConcurrencyConflictResolver<TEntity>(
        this ConcurrencyConflictResolver<TEntity> concurrencyConflictResolver)
    {
        return (oCurrent, oDb, ex) =>
        {
            if (oCurrent is TEntity eCurrent && oDb is TEntity eDb)
            {
                return concurrencyConflictResolver(eCurrent, eDb, ex);
            }

            throw new FailedToResolveConcurrencyException(ex);
        };
    }
}