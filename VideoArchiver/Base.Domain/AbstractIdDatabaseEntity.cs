namespace Domain.Base;

public abstract class AbstractIdDatabaseEntity : IIdDatabaseEntity
{
    public Guid Id { get; set; }
}

public abstract class AbstractIdDatabaseEntity<TKey> : IIdDatabaseEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}