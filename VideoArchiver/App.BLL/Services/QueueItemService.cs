using App.BLL.Base;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Exceptions;
using App.BLL.DTO.Mappers;
using App.Common.Enums;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Author = App.DAL.DTO.Entities.Author;
using Video = App.DAL.DTO.Entities.Video;

namespace App.BLL.Services;

public class QueueItemService : BaseService<QueueItemService>
{
    private readonly QueueItemMapper _queueItemMapper;
    
    public QueueItemService(ServiceUow serviceUow, ILogger<QueueItemService> logger, IMapper mapper) :
        base(serviceUow, logger, mapper)
    {
        _queueItemMapper = new QueueItemMapper(mapper);
    }

    public async Task Add(Video video, Guid submitterId, bool autoSubmit)
    {
        Uow.QueueItems.Add(new QueueItem(video, submitterId, autoSubmit));
        if (autoSubmit) await ServiceUow.AuthorizationService.AuthorizeVideoIfNotAuthorized(submitterId, video.Id);
    }

    public async Task Add(Playlist playlist, Guid submitterId, bool autoSubmit)
    {
        Uow.QueueItems.Add(new QueueItem(playlist, submitterId, autoSubmit));
        if (autoSubmit)
            await ServiceUow.AuthorizationService.AuthorizePlaylistIfNotAuthorized(submitterId, playlist.Id);
    }

    public async Task Add(Author author, Guid submitterId, bool autoSubmit)
    {
        Uow.QueueItems.Add(new QueueItem(author, submitterId, autoSubmit));
        if (autoSubmit) await ServiceUow.AuthorizationService.AuthorizeAuthorIfNotAuthorized(submitterId, author.Id);
    }

    public QueueItem Add(string idOnPlatform, EPlatform platform, EEntityType entityType, Guid submitterId, bool autoSubmit)
    {
        return Uow.QueueItems.Add(new QueueItem(idOnPlatform, platform, entityType, submitterId, autoSubmit));
    }

    public async Task<ICollection<QueueItemForApproval>> GetAllAwaitingApprovalAsync()
    {
        return (await Uow.QueueItems.GetAllAwaitingApprovalAsync()).Select(q => _queueItemMapper.Map(q)!).ToList();
    }

    public async Task DeleteAsync(Guid id)
    {
        await Uow.QueueItems.RemoveAsync(id);
    }

    public async Task ApproveAsync(Guid id, Guid approvedById)
    {
        var queueItem = await Uow.QueueItems.GetByIdAsync(id) ?? throw new NotFoundException();
        queueItem.ApprovedById = approvedById;
        queueItem.ApprovedAt = DateTime.UtcNow;
        Uow.QueueItems.Update(queueItem);
        Uow.SavedChanges += (_, _) => ServiceContext.QueueNewQueueItem(id);
    }
}