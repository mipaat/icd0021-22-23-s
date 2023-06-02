using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class QueueItemRepository : BaseAppEntityRepository<App.Domain.QueueItem, QueueItem>, IQueueItemRepository
{
    public QueueItemRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext,
        mapper, uow)
    {
    }

    protected override TQueryable IncludeDefaults<TQueryable>(TQueryable queryable)
    {
        queryable
            .Include(e => e.Video)
            .Include(e => e.Playlist)
            .Include(e => e.Author)
            .Include(e => e.AddedBy)
            .Include(e => e.ApprovedBy);
        return queryable;
    }

    protected override Domain.QueueItem AfterMap(QueueItem entity, Domain.QueueItem mapped)
    {
        if (entity.VideoId != null)
        {
            var trackedVideo = Uow.Videos.GetTrackedEntity(entity.VideoId.Value);
            if (trackedVideo != null)
            {
                mapped.Video = trackedVideo;
            }
        }

        if (entity.PlaylistId != null)
        {
            var trackedPlaylist = Uow.Playlists.GetTrackedEntity(entity.PlaylistId.Value);
            if (trackedPlaylist != null)
            {
                mapped.Playlist = trackedPlaylist;
            }
        }

        if (entity.AuthorId != null)
        {
            var trackedAuthor = Uow.Authors.GetTrackedEntity(entity.AuthorId.Value);
            if (trackedAuthor != null)
            {
                mapped.Author = trackedAuthor;
            }
        }

        return mapped;
    }

    public async Task<ICollection<QueueItem>> GetAllAwaitingApprovalAsync()
    {
        return await GetAllAsync(q => q.ApprovedAt == null);
    }

    public async Task<ICollection<Guid>> GetAllApprovedNotCompletedAsync()
    {
        return await Entities.Where(q => q.ApprovedAt != null && q.CompletedAt == null)
            .Select(q => q.Id)
            .ToListAsync();
    }
}