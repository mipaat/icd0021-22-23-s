using System.ComponentModel.DataAnnotations;

namespace Domain.Base;

public abstract class AbstractIdDatabaseEntity : AbstractIdDatabaseEntity<Guid>, IIdDatabaseEntity
{
}

public abstract class AbstractIdDatabaseEntity<TKey> : IIdDatabaseEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }

    [Timestamp]
    public uint RowVersion { get; set; } // NB! Provider-specific? Works with Postgres
}