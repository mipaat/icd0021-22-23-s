using App.Common;
using App.Common.Enums;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class EntityAccessPermissionRepository :
    BaseAppEntityRepository<Domain.EntityAccessPermission, EntityAccessPermission>, IEntityAccessPermissionRepository
{
    public EntityAccessPermissionRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) :
        base(dbContext, mapper, uow)
    {
    }

    protected override Domain.EntityAccessPermission AfterMap(EntityAccessPermission entity,
        Domain.EntityAccessPermission mapped)
    {
        var user = Uow.Users.GetTrackedEntity(entity.UserId);
        if (user != null) mapped.User = user;
        if (entity.VideoId != null)
        {
            var video = Uow.Videos.GetTrackedEntity(entity.VideoId.Value);
            if (video != null) mapped.Video = video;
        }

        if (entity.PlaylistId != null)
        {
            var playlist = Uow.Playlists.GetTrackedEntity(entity.PlaylistId.Value);
            if (playlist != null) mapped.Playlist = playlist;
        }

        if (entity.AuthorId != null)
        {
            var author = Uow.Authors.GetTrackedEntity(entity.AuthorId.Value);
            if (author != null) mapped.Author = author;
        }

        return mapped;
    }

    public async Task<bool> AllowedToAccessVideoAsync(Guid userId, Guid videoId)
    {
        return await DbContext.Videos.Where(v => v.Id == videoId &&
                                    (v.InternalPrivacyStatus != EPrivacyStatus.Private ||
                                     Entities.Any(e => e.UserId == userId && e.VideoId == videoId) ||
                                     DbContext.PlaylistVideos.Any(p =>
                                         p.VideoId == videoId && Entities.Any(e =>
                                             e.UserId == userId && e.PlaylistId == p.PlaylistId)))).AnyAsync();
    }

    public async Task<bool> AllowedToAccessVideoAnonymouslyAsync(Guid videoId)
    {
        return await DbContext.Videos.AnyAsync(
            v => v.Id == videoId && v.InternalPrivacyStatus != EPrivacyStatus.Private);
    }
}