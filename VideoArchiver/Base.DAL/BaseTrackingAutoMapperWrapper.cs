using AutoMapper;
using Domain.Base;

namespace Base.DAL;

public class BaseTrackingAutoMapperWrapper : BaseTrackingAutoMapperWrapper<Guid>
{
    public BaseTrackingAutoMapperWrapper(IMapper mapper) : base(mapper)
    {
    }
}

public class BaseTrackingAutoMapperWrapper<TKey> : AbstractTrackingAutoMapperWrapper<TKey> where TKey : struct, IEquatable<TKey>
{
    public BaseTrackingAutoMapperWrapper(IMapper mapper) : base(mapper)
    {
    }

    private readonly Dictionary<Type, Dictionary<TKey, IIdDatabaseEntity<TKey>>> _trackedIdEntities = new();

    private readonly Dictionary<Type, HashSet<object>> _trackedEntities = new();

    protected override TEntity? GetTrackedEntityOrDefault<TEntity>(TEntity entity) where TEntity : class
    {
        var entityType = typeof(TEntity);
        if (IsIdDatabaseEntity(entityType))
        {
            var idDatabaseEntity = (IIdDatabaseEntity<TKey>)entity;
            return (TEntity?)_trackedIdEntities.GetValueOrDefault(entityType)?.GetValueOrDefault(idDatabaseEntity.Id);
        }

        return (TEntity?)_trackedEntities.GetValueOrDefault(entityType)
            ?.SingleOrDefault(e => ReferenceEquals(e, entity));
    }

    protected override void SetTrackedEntity<TEntity>(TEntity entity)
    {
        var entityType = typeof(TEntity);
        if (IsIdDatabaseEntity(entityType))
        {
            var idDatabaseEntity = (IIdDatabaseEntity<TKey>)entity;
            if (!_trackedIdEntities.ContainsKey(entityType))
            {
                _trackedIdEntities[entityType] = new Dictionary<TKey, IIdDatabaseEntity<TKey>>();
            }

            _trackedIdEntities[entityType][idDatabaseEntity.Id] = idDatabaseEntity;
            return;
        }

        if (!_trackedEntities.ContainsKey(entityType))
        {
            _trackedEntities[entityType] = new HashSet<object>();
        }

        _trackedEntities[entityType].Add(entity);
    }
}