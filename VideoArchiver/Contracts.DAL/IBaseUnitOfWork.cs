namespace Contracts.DAL;

public interface IBaseUnitOfWork : IDisposable, IAsyncDisposable
{
    public Task<int> SaveChangesAsync();

    public event EventHandler SavedChanges;

    public void AddConcurrencyConflictResolver<TEntity>(
        ConcurrencyConflictResolverAsync<TEntity> concurrencyConflictResolver);
    public void AddConcurrencyConflictResolver<TEntity>(
        ConcurrencyConflictResolver<TEntity> concurrencyConflictResolver);
}