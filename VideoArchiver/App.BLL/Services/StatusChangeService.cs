using App.BLL.Base;
using App.Domain;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class StatusChangeService : BaseService<StatusChangeService>
{
    public StatusChangeService(ServiceUow serviceUow, ILogger<StatusChangeService> logger) : base(serviceUow, logger)
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