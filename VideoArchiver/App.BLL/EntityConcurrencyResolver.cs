using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.Domain;
using AutoMapper;
using Base.Mapping;
using Contracts.DAL;

namespace App.BLL;

public class EntityConcurrencyResolver : IEntityConcurrencyResolver
{
    private readonly IEntityUpdateService _entityUpdateService;
    private readonly BaseMapper<Video, App.DAL.DTO.Entities.Video> _videoMapper;
    private readonly BaseMapper<Comment, App.DAL.DTO.Entities.Comment> _commentMapper;

    public EntityConcurrencyResolver(IEntityUpdateService entityUpdateService, IMapper mapper)
    {
        _entityUpdateService = entityUpdateService;
        _videoMapper = new BaseMapper<Video, App.DAL.DTO.Entities.Video>(mapper);
        _commentMapper = new BaseMapper<Comment, DAL.DTO.Entities.Comment>(mapper);
    }

    public async Task<Video> ResolveVideoConcurrency(Video currentVideo, Video? dbVideo, Exception sourceException)
    {
        return _videoMapper.Map(await ResolveEntityConcurrency(
            _videoMapper.Map(currentVideo)!,
            _videoMapper.Map(dbVideo)!,
            _entityUpdateService.UpdateVideo,
            sourceException))!;
    }

    public async Task<Comment> ResolveCommentConcurrency(Comment currentComment, Comment? dbComment,
        Exception sourceException)
    {
        return _commentMapper.Map(await ResolveEntityConcurrency(
            _commentMapper.Map(currentComment)!,
            _commentMapper.Map(dbComment)!,
            _entityUpdateService.UpdateComment,
            sourceException))!;
    }

    private async Task<TEntity> ResolveEntityConcurrency<TEntity>(TEntity currentEntity, TEntity? dbEntity,
        Func<TEntity, TEntity, Task> updateFunc, Exception sourceException)
        where TEntity : App.DAL.DTO.Base.BaseArchiveEntityNonMonitored
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