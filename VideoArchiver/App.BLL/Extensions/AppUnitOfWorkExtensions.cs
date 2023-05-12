using App.BLL.Services;
using App.Contracts.DAL;
using App.Domain;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace App.BLL.Extensions;

public static class AppUnitOfWorkExtensions
{
    public static IAppUnitOfWork AddDefaultConcurrencyConflictResolvers(this IAppUnitOfWork uow,
        IServiceProvider services)
    {
        var entityConcurrencyResolver =
            new EntityConcurrencyResolver(new EntityUpdateService(null), services.GetRequiredService<IMapper>());
        uow.AddConcurrencyConflictResolver<Video>(async (currentVideo, dbVideo, e) =>
            await entityConcurrencyResolver.ResolveVideoConcurrency(currentVideo, dbVideo, e));
        uow.AddConcurrencyConflictResolver<Comment>(async (currentComment, dbComment, e) =>
            await entityConcurrencyResolver.ResolveCommentConcurrency(currentComment, dbComment, e));
        return uow;
    }
}