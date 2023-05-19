using App.Contracts.DAL;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.Base;

public abstract class BaseService<TService>
{
    public readonly ServiceUow ServiceUow;
    protected readonly ILogger<TService> Logger;

    protected BaseService(ServiceUow serviceUow, ILogger<TService> logger, IMapper mapper)
    {
        ServiceUow = serviceUow;
        Logger = logger;
    }

    protected IAppUnitOfWork Uow => ServiceUow.Uow;
    protected ServiceContext ServiceContext => ServiceUow.ServiceContext;
}