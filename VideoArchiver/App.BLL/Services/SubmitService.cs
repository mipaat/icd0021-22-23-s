using System.Security.Claims;
using System.Security.Principal;
using App.BLL.Base;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.DTO.Entities;
using App.BLL.Exceptions;
using App.Common;
using App.Common.Enums;
using App.DAL.DTO.Entities;
using Base.WebHelpers;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class SubmitService : BaseService<SubmitService>, ISubmitService
{
    private readonly IEnumerable<IPlatformSubmissionHandler> _platformSubmissionHandlers;

    public const string AllowedToSubmitRoles = $"{RoleNames.AdminOrSuperAdmin},{RoleNames.Helper}";

    private static readonly List<string> AllowedToAutoSubmitRoles = new()
    {
        RoleNames.Admin,
        RoleNames.SuperAdmin
    };

    public SubmitService(IServiceUow serviceUow, ILogger<SubmitService> logger,
        IEnumerable<IPlatformSubmissionHandler> platformSubmissionHandlers) :
        base(serviceUow, logger)
    {
        _platformSubmissionHandlers = platformSubmissionHandlers;
    }

    private static bool IsAllowedToAutoSubmit(IPrincipal user)
    {
        return AllowedToAutoSubmitRoles.Any(user.IsInRole);
    }

    public async Task<UrlSubmissionResult> SubmitGenericUrlAsync(string url, ClaimsPrincipal user, bool submitPlaylist)
    {
        return await SubmitGenericUrlAsync(url, user.GetUserId(), IsAllowedToAutoSubmit(user), submitPlaylist);
    }

    private async Task<UrlSubmissionResult> SubmitGenericUrlAsync(string url, Guid submitterId, bool autoSubmit, bool submitPlaylist)
    {
        foreach (var urlHandler in _platformSubmissionHandlers)
        {
            if (urlHandler.IsPlatformUrl(url))
            {
                return await urlHandler.SubmitUrl(url, submitterId, autoSubmit, submitPlaylist);
            }
        }

        throw new UnrecognizedUrlException(url);
    }

    public async Task SubmitQueueItemAsync(QueueItem queueItem)
    {
        var handler = _platformSubmissionHandlers
            .FirstOrDefault(h => h.CanHandle(queueItem.Platform, queueItem.EntityType));
        if (handler == null)
        {
            Uow.QueueItems.Remove(queueItem);
            return;
        }

        switch (queueItem.EntityType)
        {
            case EEntityType.Video:
                var video = await handler.SubmitVideo(queueItem.IdOnPlatform);
                queueItem.VideoId = video.Id;
                if (queueItem.GrantAccess)
                    await ServiceUow.AuthorizationService.AuthorizeVideoIfNotAuthorized(queueItem.AddedById, video.Id);
                break;
            case EEntityType.Playlist:
                var playlist = await handler.SubmitPlaylist(queueItem.IdOnPlatform);
                queueItem.PlaylistId = playlist.Id;
                if (queueItem.GrantAccess)
                    await ServiceUow.AuthorizationService.AuthorizePlaylistIfNotAuthorized(queueItem.AddedById,
                        playlist.Id);
                break;
            case EEntityType.Author:
                var author = await handler.SubmitAuthor(queueItem.IdOnPlatform);
                queueItem.AuthorId = author.Id;
                if (queueItem.GrantAccess)
                    await ServiceUow.AuthorizationService.AuthorizeAuthorIfNotAuthorized(queueItem.AddedById,
                        author.Id);
                break;
            default:
                throw new ArgumentException(
                    $"{typeof(QueueItem)} {queueItem.Id} has invalid {typeof(EEntityType)}: {queueItem.EntityType}");
        }

        queueItem.CompletedAt = DateTime.UtcNow;
        Uow.QueueItems.Update(queueItem);
    }
}