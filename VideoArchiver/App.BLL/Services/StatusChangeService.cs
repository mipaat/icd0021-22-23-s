using App.BLL.Base;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.DAL.DTO.Entities;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class StatusChangeService : BaseService<StatusChangeService>, IStatusChangeService
{
    public StatusChangeService(IServiceUow serviceUow, ILogger<StatusChangeService> logger) : base(serviceUow, logger)
    {
    }

    public Task Push(StatusChangeEvent statusChangeEvent)
    {
        if (statusChangeEvent.IsChanged)
        {
            Uow.StatusChangeEvents.Add(statusChangeEvent);
        }
        // TODO: Notifications
        // TODO: Non-polling based notifications?

        return Task.CompletedTask;
    }
}