using App.BLL.Contracts.Services;
using App.DAL.Contracts;
using Contracts.BLL;
using Microsoft.Extensions.Configuration;

namespace App.BLL.Contracts;

public interface IServiceUow : IAppUowContainer
{
    public IServiceProvider Services { get; }
    public IConfiguration Config { get; }
    
    public IAppUnitOfWork Uow { get; }
    public IServiceContext ServiceContext { get; }

    public IStatusChangeService StatusChangeService { get; }
    public IEntityUpdateService EntityUpdateService { get; }
    public IImageService ImageService { get; }
    public IAuthorizationService AuthorizationService { get; }
    public IQueueItemService QueueItemService { get; }
    public ISubmitService SubmitService { get; }
    public ICategoryService CategoryService { get; }
    public IVideoPresentationService VideoPresentationService { get; }
    public ICommentService CommentService { get; }
    public IAuthorService AuthorService { get; }
    public IVideoService VideoService { get; }
}