using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
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
        if (entity.Video != null)
        {
            var trackedVideo = Uow.Videos.GetTrackedEntity(entity.Video);
            if (trackedVideo != null)
            {
                mapped.Video = Uow.Videos.Map(entity.Video, trackedVideo);
            }
        }

        if (entity.Playlist != null)
        {
            var trackedPlaylist = Uow.Playlists.GetTrackedEntity(entity.Playlist);
            if (trackedPlaylist != null)
            {
                mapped.Playlist = Uow.Playlists.Map(entity.Playlist, trackedPlaylist);
            }
        }

        if (entity.Author != null)
        {
            var trackedAuthor = Uow.Authors.GetTrackedEntity(entity.Author);
            if (trackedAuthor != null)
            {
                mapped.Author = Uow.Authors.Map(entity.Author, trackedAuthor);
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