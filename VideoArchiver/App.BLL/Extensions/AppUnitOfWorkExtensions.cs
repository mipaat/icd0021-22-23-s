using App.BLL.Services;
using App.Contracts.DAL;
using App.Domain;

namespace App.BLL.Extensions;

public static class AppUnitOfWorkExtensions
{
    public static IAppUnitOfWork AddDefaultConcurrencyConflictResolvers(this IAppUnitOfWork uow, IServiceProvider services)
    {
        var entityConcurrencyResolver = new EntityConcurrencyResolver(new EntityUpdateService(null));
        uow.AddConcurrencyConflictResolver<Video>(async (currentVideo, dbVideo, e) =>
            await entityConcurrencyResolver.ResolveVideoConcurrency(currentVideo, dbVideo, e));
        uow.AddConcurrencyConflictResolver<Comment>(async (currentComment, dbComment, e) =>
            await entityConcurrencyResolver.ResolveCommentConcurrency(currentComment, dbComment, e));
        return uow;
    }
}