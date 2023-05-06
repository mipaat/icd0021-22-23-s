using App.Domain;
using App.Domain.Base;
using Contracts.DAL;

namespace App.BLL;

public class EntityConcurrencyResolver
{
    private readonly EntityUpdateHandler _entityUpdateHandler;

    public EntityConcurrencyResolver(EntityUpdateHandler entityUpdateHandler)
    {
        _entityUpdateHandler = entityUpdateHandler;
    }

    public Video ResolveVideoConcurrency(Video currentVideo, Video? dbVideo, Exception sourceException)
    {
        return ResolveEntityConcurrency(currentVideo, dbVideo, _entityUpdateHandler.UpdateVideo, sourceException);
    }

    public Comment ResolveCommentConcurrency(Comment currentComment, Comment? dbComment, Exception sourceException)
    {
        return ResolveEntityConcurrency(currentComment, dbComment, _entityUpdateHandler.UpdateComment, sourceException);
    }

    private TEntity ResolveEntityConcurrency<TEntity>(TEntity currentEntity, TEntity? dbEntity, Action<TEntity, TEntity> updateFunc, Exception sourceException)
        where TEntity : BaseArchiveEntityNonMonitored
    {
        if (dbEntity == null) return currentEntity;
        if (Utils.Utils.LaterThan(currentEntity.UpdatedAt, dbEntity.UpdatedAt))
        {
            updateFunc(dbEntity, currentEntity);
            return dbEntity;
        }

        if (Utils.Utils.LaterThan(dbEntity.UpdatedAt, currentEntity.UpdatedAt))
        {
            updateFunc(currentEntity, dbEntity);
            return currentEntity;
        }

        if (Utils.Utils.LaterThan(currentEntity.LastSuccessfulFetchOfficial, dbEntity.LastSuccessfulFetchOfficial))
        {
            updateFunc(dbEntity, currentEntity);
            return dbEntity;
        }

        if (Utils.Utils.LaterThan(dbEntity.LastSuccessfulFetchOfficial, currentEntity.LastSuccessfulFetchOfficial))
        {
            updateFunc(currentEntity, dbEntity);
            return currentEntity;
        }

        if (Utils.Utils.LaterThan(currentEntity.LastSuccessfulFetchUnofficial, dbEntity.LastSuccessfulFetchUnofficial))
        {
            updateFunc(dbEntity, currentEntity);
            return dbEntity;
        }

        if (Utils.Utils.LaterThan(dbEntity.LastSuccessfulFetchUnofficial, currentEntity.LastSuccessfulFetchUnofficial))
        {
            updateFunc(currentEntity, dbEntity);
            return currentEntity;
        }

        throw new FailedToResolveConcurrencyException(sourceException);
    }
}