using App.BLL.Base;
using App.BLL.Services;
using App.Contracts.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL;

public class ServiceUow : BaseAppUowContainer
{
    public readonly IServiceProvider Services;
    public readonly IConfiguration Config;

    public ServiceUow(IAppUnitOfWork uow, IServiceProvider services, IConfiguration config) : base(uow)
    {
        Services = services;
        Config = config;
    }

    private ServiceContext? _serviceContext;
    public ServiceContext ServiceContext => _serviceContext ??= Services.GetRequiredService<ServiceContext>();

    private StatusChangeService? _statusChangeService;

    public StatusChangeService StatusChangeService =>
        _statusChangeService ??= Services.GetRequiredService<StatusChangeService>();

    private EntityUpdateService? _entityUpdateService;

    public EntityUpdateService EntityUpdateService =>
        _entityUpdateService ??= Services.GetRequiredService<EntityUpdateService>();

    private ImageService? _imageService;
    public ImageService ImageService => _imageService ??= Services.GetRequiredService<ImageService>();

    private AuthorizationService? _authorizationService;

    public AuthorizationService AuthorizationService =>
        _authorizationService ??= Services.GetRequiredService<AuthorizationService>();

    private QueueItemService? _queueItemService;
    public QueueItemService QueueItemService => _queueItemService ??= Services.GetRequiredService<QueueItemService>();

    private SubmitService? _submitService;
    public SubmitService SubmitService => _submitService ??= Services.GetRequiredService<SubmitService>();
}