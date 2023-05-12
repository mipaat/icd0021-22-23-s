using App.Contracts.DAL;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.Base;

public abstract class BaseService<TService>
{
    protected readonly ServiceUow ServiceUow;
    protected readonly ILogger<TService> Logger;
    protected readonly IMapper _mapper;

    protected BaseService(ServiceUow serviceUow, ILogger<TService> logger, IMapper mapper)
    {
        ServiceUow = serviceUow;
        Logger = logger;
        _mapper = mapper;
    }

    protected IAppUnitOfWork Uow => ServiceUow.Uow;
}