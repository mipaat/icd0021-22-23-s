namespace Contracts.DAL;

public delegate TEntity? ConcurrencyConflictResolver<TEntity>(
    TEntity currentEntity, TEntity? dbEntity, Exception exception);

public delegate Task<TEntity?> ConcurrencyConflictResolverAsync<TEntity>(
    TEntity currentEntity, TEntity? dbEntity, Exception exception);

public delegate object? ConcurrencyConflictResolverUntyped(
    object currentEntity, object? dbEntity, Exception exception);

public delegate Task<object?> ConcurrencyConflictResolverUntypedAsync(
    object currentEntity, object? dbEntity, Exception exception);