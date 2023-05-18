using System.Security.Claims;
using System.Security.Principal;
using App.BLL.DTO;
using App.Common;
using App.Contracts.DAL;
using App.DAL.DTO.Entities;
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

    private void AuthorizeVideo(Guid userId, Guid videoId)
    {
        _uow.EntityAccessPermissions.Add(new EntityAccessPermission
        {
            UserId = userId,
            VideoId = videoId,
        });
    }

    public async Task AuthorizeVideoIfNotAuthorized(Guid userId, Guid videoId)
    {
        if (await _uow.EntityAccessPermissions.VideoPermissionExistsAsync(userId, videoId)) return;
        AuthorizeVideo(userId, videoId);
    }

    private void AuthorizePlaylist(Guid userId, Guid playlistId)
    {
        _uow.EntityAccessPermissions.Add(new EntityAccessPermission
        {
            UserId = userId,
            PlaylistId = playlistId,
        });
    }

    public async Task AuthorizePlaylistIfNotAuthorized(Guid userId, Guid playlistId)
    {
        if (await _uow.EntityAccessPermissions.PlaylistPermissionExistsAsync(userId, playlistId)) return;
        AuthorizePlaylist(userId, playlistId);
    }

    private void AuthorizeAuthor(Guid userId, Guid authorId)
    {
        _uow.EntityAccessPermissions.Add(new EntityAccessPermission
        {
            UserId = userId,
            AuthorId = authorId,
        });
    }

    public async Task AuthorizeAuthorIfNotAuthorized(Guid userId, Guid authorId)
    {
        if (await _uow.EntityAccessPermissions.AuthorPermissionExistsAsync(userId, authorId)) return;
        AuthorizeAuthor(userId, authorId);
    }
}