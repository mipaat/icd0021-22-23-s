using App.BLL.Services;
using App.Contracts.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL;

public class ServiceUow
{
    public readonly IAppUnitOfWork Uow;
    public readonly IServiceProvider Services;
    public readonly IConfiguration Config;

    public ServiceUow(IAppUnitOfWork uow, IServiceProvider services, IConfiguration config)
    {
        Uow = uow;
        Services = services;
        Config = config;
    }

    private StatusChangeService? _statusChangeService;

    public StatusChangeService StatusChangeService =>
        _statusChangeService ??= Services.GetRequiredService<StatusChangeService>();

    private EntityUpdateService? _entityUpdateService;

    public EntityUpdateService EntityUpdateService =>
        _entityUpdateService ??= Services.GetRequiredService<EntityUpdateService>();

    private ImageService? _imageService;
    public ImageService ImageService => _imageService ??= Services.GetRequiredService<ImageService>();
}