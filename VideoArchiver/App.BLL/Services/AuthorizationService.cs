using System.Security.Claims;
using System.Security.Principal;
using App.BLL.DTO;
using App.Contracts.DAL;
using Base.WebHelpers;

namespace App.BLL.Services;

public class AuthorizationService
{
    private readonly IAppUnitOfWork _uow;

    public AuthorizationService(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    private static bool IsRoleAllowed(IPrincipal user) =>
        user.IsInRole(RoleNames.Admin) || user.IsInRole(RoleNames.Helper);

    public async Task<bool> IsAllowedToAccessVideo(ClaimsPrincipal user, Guid videoId)
    {
        if (IsRoleAllowed(user)) return true;
        var userId = user.GetUserIdIfExists();
        if (userId == null) return await _uow.EntityAccessPermissions.AllowedToAccessVideoAnonymouslyAsync(videoId);
        return await _uow.EntityAccessPermissions.AllowedToAccessVideoAsync(userId.Value, videoId);
    }
}