using System.Security.Claims;
using System.Security.Principal;
using App.BLL.Base;
using App.BLL.DTO;
using App.BLL.DTO.Contracts;
using App.BLL.DTO.Entities;
using App.BLL.Exceptions;
using App.Common.Enums;
using App.Contracts.DAL;
using App.DAL.DTO.Entities;
using Base.WebHelpers;

namespace App.BLL.Services;

public class SubmitService : BaseAppUowContainer
{
    private readonly IEnumerable<IPlatformSubmissionHandler> _platformSubmissionHandlers;

    public const string AllowedToSubmitRoles = $"{RoleNames.Admin},{RoleNames.Helper}";

    private static readonly List<string> AllowedToAutoSubmitRoles = new()
    {
        RoleNames.Admin
    };

    public SubmitService(IEnumerable<IPlatformSubmissionHandler> platformSubmissionHandlers,
        IAppUnitOfWork uow) : base(uow)
    {
        _platformSubmissionHandlers = platformSubmissionHandlers;
    }

    private static bool IsAllowedToAutoSubmit(IPrincipal user)
    {
        return AllowedToAutoSubmitRoles.Any(user.IsInRole);
    }

    public async Task<UrlSubmissionResults> SubmitGenericUrlAsync(string url, ClaimsPrincipal user)
    {
        return await SubmitGenericUrlAsync(url, user.GetUserId(), IsAllowedToAutoSubmit(user));
    }

    private async Task<UrlSubmissionResults> SubmitGenericUrlAsync(string url, Guid submitterId, bool autoSubmit)
    {
        foreach (var urlHandler in _platformSubmissionHandlers)
        {
            if (urlHandler.IsPlatformUrl(url))
            {
                return await urlHandler.SubmitUrl(url, submitterId, autoSubmit);
            }
        }

        throw new UnrecognizedUrlException(url);
    }

    public async Task SubmitQueueItem(QueueItem queueItem)
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
                queueItem.Video = video;
                queueItem.VideoId = video.Id;
                break;
            case EEntityType.Playlist:
                var playlist = await handler.SubmitPlaylist(queueItem.IdOnPlatform);
                queueItem.Playlist = playlist;
                queueItem.PlaylistId = playlist.Id;
                break;
            case EEntityType.Author:
                var author = await handler.SubmitAuthor(queueItem.IdOnPlatform);
                queueItem.Author = author;
                queueItem.AuthorId = author.Id;
                break;
            default:
                throw new ArgumentException(
                    $"{typeof(QueueItem)} {queueItem.Id} has invalid {typeof(EEntityType)}: {queueItem.EntityType}");
        }

        queueItem.CompletedAt = DateTime.UtcNow;
        Uow.QueueItems.Update(queueItem);
    }
}