using App.BLL.Contracts;
using App.DAL.Contracts;
using Microsoft.Extensions.Logging;

namespace App.BLL.Base;

public abstract class BaseService<TService> : IBaseService
{
    public IServiceUow ServiceUow { get; }
    protected readonly ILogger<TService> Logger;

    protected BaseService(IServiceUow serviceUow, ILogger<TService> logger)
    {
        ServiceUow = serviceUow;
        Logger = logger;
    }

    protected IAppUnitOfWork Uow => ServiceUow.Uow;
    protected IServiceContext ServiceContext => ServiceUow.ServiceContext;
}