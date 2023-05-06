using App.Contracts.DAL;
using App.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Extensions;

public static class AppUnitOfWorkExtensions
{
    public static IAppUnitOfWork AddDefaultConcurrencyConflictResolvers(this IAppUnitOfWork uow, IServiceProvider services)
    {
        var entityConcurrencyResolver = services.GetRequiredService<EntityConcurrencyResolver>();
        uow.AddConcurrencyConflictResolver<Video>((currentVideo, dbVideo, e) =>
            entityConcurrencyResolver.ResolveVideoConcurrency(currentVideo, dbVideo, e));
        uow.AddConcurrencyConflictResolver<Comment>((currentComment, dbComment, e) =>
            entityConcurrencyResolver.ResolveCommentConcurrency(currentComment, dbComment, e));
        return uow;
    }
}