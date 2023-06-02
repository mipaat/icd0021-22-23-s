using App.BLL.Base;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.DAL.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL;

public class ServiceUow : BaseAppUowContainer, IServiceUow
{
    public IServiceProvider Services { get; }
    public IConfiguration Config { get; }

    public ServiceUow(IAppUnitOfWork uow, IServiceProvider services, IConfiguration config) : base(uow)
    {
        Services = services;
        Config = config;
    }

    private IServiceContext? _serviceContext;
    public IServiceContext ServiceContext => _serviceContext ??= Services.GetRequiredService<IServiceContext>();

    private IStatusChangeService? _statusChangeService;

    public IStatusChangeService StatusChangeService =>
        _statusChangeService ??= Services.GetRequiredService<IStatusChangeService>();

    private IEntityUpdateService? _entityUpdateService;

    public IEntityUpdateService EntityUpdateService =>
        _entityUpdateService ??= Services.GetRequiredService<IEntityUpdateService>();

    private IImageService? _imageService;
    public IImageService ImageService => _imageService ??= Services.GetRequiredService<IImageService>();

    private IAuthorizationService? _authorizationService;

    public IAuthorizationService AuthorizationService =>
        _authorizationService ??= Services.GetRequiredService<IAuthorizationService>();

    private IQueueItemService? _queueItemService;
    public IQueueItemService QueueItemService => _queueItemService ??= Services.GetRequiredService<IQueueItemService>();

    private ISubmitService? _submitService;
    public ISubmitService SubmitService => _submitService ??= Services.GetRequiredService<ISubmitService>();

    private ICategoryService? _categoryService;
    public ICategoryService CategoryService => _categoryService ??= Services.GetRequiredService<ICategoryService>();

    private IVideoPresentationService? _videoPresentationService;

    public IVideoPresentationService VideoPresentationService =>
        _videoPresentationService ??= Services.GetRequiredService<IVideoPresentationService>();

    private ICommentService? _commentService;
    public ICommentService CommentService => _commentService ??= Services.GetRequiredService<ICommentService>();

    private IAuthorService? _authorService;
    public IAuthorService AuthorService => _authorService ??= Services.GetRequiredService<IAuthorService>();

    private IVideoService? _videoService;
    public IVideoService VideoService => _videoService ??= Services.GetRequiredService<IVideoService>();
}