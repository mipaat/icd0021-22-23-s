using System.Security.Claims;
using System.Security.Principal;
using App.BLL.Base;
using App.BLL.Exceptions;
using App.Contracts.DAL;
using App.BLL.DTO;
using App.BLL.DTO.Entities;
using Base.WebHelpers;

namespace App.BLL;

public class UrlSubmissionHandler : BaseAppUowContainer
{
    private readonly IEnumerable<IPlatformUrlSubmissionHandler> _platformUrlSubmissionHandlers;

    public const string AllowedToSubmitRoles = $"{RoleNames.Admin},{RoleNames.Helper}";

    private static readonly List<string> AllowedToAutoSubmitRoles = new()
    {
        RoleNames.Admin
    };

    public UrlSubmissionHandler(IEnumerable<IPlatformUrlSubmissionHandler> platformUrlSubmissionHandlers,
        IAppUnitOfWork uow) : base(uow)
    {
        _platformUrlSubmissionHandlers = platformUrlSubmissionHandlers;
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
        foreach (var urlHandler in _platformUrlSubmissionHandlers)
        {
            if (urlHandler.IsPlatformUrl(url))
            {
                return await urlHandler.SubmitUrl(url, submitterId, autoSubmit);
            }
        }

        throw new UnrecognizedUrlException(url);
    }
}