using App.BLL.Base;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class StatusChangeService : BaseService<StatusChangeService>
{
    public StatusChangeService(ServiceUow serviceUow, ILogger<StatusChangeService> logger, IMapper mapper) : base(serviceUow, logger, mapper)
    {
    }

    public Task Push(StatusChangeEvent statusChangeEvent)
    {
        Uow.StatusChangeEvents.Add(statusChangeEvent);
        // TODO: Notifications
        // TODO: Non-polling based notifications?

        return Task.CompletedTask;
    }
}