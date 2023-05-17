using App.BLL.Base;
using App.Common.Enums;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class QueueItemService : BaseService<QueueItemService>
{
    public QueueItemService(ServiceUow serviceUow, ILogger<QueueItemService> logger, IMapper mapper) :
        base(serviceUow, logger, mapper)
    {
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
}