namespace Domain.Base;

public abstract class AbstractIdDatabaseEntity : IIdDatabaseEntity
{
    public Guid Id { get; set; } = new();
}

public abstract class AbstractIdDatabaseEntity<TKey> : IIdDatabaseEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}