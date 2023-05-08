using App.Contracts.DAL;
using Microsoft.Extensions.Logging;

namespace App.BLL.Base;

public abstract class BaseService<TService>
{
    protected readonly ServiceUow ServiceUow;
    protected readonly ILogger<TService> Logger;

    protected BaseService(ServiceUow serviceUow, ILogger<TService> logger)
    {
        ServiceUow = serviceUow;
        Logger = logger;
    }

    protected IAppUnitOfWork Uow => ServiceUow.Uow;
}