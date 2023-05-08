using App.BLL.Services;
using App.Domain;
using App.Domain.Base;
using Contracts.DAL;

namespace App.BLL;

public class EntityConcurrencyResolver
{
    private readonly EntityUpdateService _entityUpdateService;

    public EntityConcurrencyResolver(EntityUpdateService entityUpdateService)
    {
        _entityUpdateService = entityUpdateService;
    }

    public async Task<Video> ResolveVideoConcurrency(Video currentVideo, Video? dbVideo, Exception sourceException)
    {
        return await ResolveEntityConcurrency(currentVideo, dbVideo, _entityUpdateService.UpdateVideo, sourceException);
    }

    public async Task<Comment> ResolveCommentConcurrency(Comment currentComment, Comment? dbComment, Exception sourceException)
    {
        return await ResolveEntityConcurrency(currentComment, dbComment, _entityUpdateService.UpdateComment, sourceException);
    }

    private async Task<TEntity> ResolveEntityConcurrency<TEntity>(TEntity currentEntity, TEntity? dbEntity, Func<TEntity, TEntity, Task> updateFunc, Exception sourceException)
        where TEntity : BaseArchiveEntityNonMonitored
    {
        if (dbEntity == null) return currentEntity;
        if (Utils.Utils.LaterThan(currentEntity.UpdatedAt, dbEntity.UpdatedAt))
        {
            await updateFunc(dbEntity, currentEntity);
            return dbEntity;
        }

        if (Utils.Utils.LaterThan(dbEntity.UpdatedAt, currentEntity.UpdatedAt))
        {
            await updateFunc(currentEntity, dbEntity);
            return currentEntity;
        }

        if (Utils.Utils.LaterThan(currentEntity.LastSuccessfulFetchOfficial, dbEntity.LastSuccessfulFetchOfficial))
        {
            await updateFunc(dbEntity, currentEntity);
            return dbEntity;
        }

        if (Utils.Utils.LaterThan(dbEntity.LastSuccessfulFetchOfficial, currentEntity.LastSuccessfulFetchOfficial))
        {
            await updateFunc(currentEntity, dbEntity);
            return currentEntity;
        }

        if (Utils.Utils.LaterThan(currentEntity.LastSuccessfulFetchUnofficial, dbEntity.LastSuccessfulFetchUnofficial))
        {
            await updateFunc(dbEntity, currentEntity);
            return dbEntity;
        }

        if (Utils.Utils.LaterThan(dbEntity.LastSuccessfulFetchUnofficial, currentEntity.LastSuccessfulFetchUnofficial))
        {
            await updateFunc(currentEntity, dbEntity);
            return currentEntity;
        }

        throw new FailedToResolveConcurrencyException(sourceException);
    }
}