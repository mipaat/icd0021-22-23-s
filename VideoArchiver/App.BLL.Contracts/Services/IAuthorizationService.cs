using System.Security.Claims;
using App.Common.Enums;

namespace App.BLL.Contracts.Services;

public interface IAuthorizationService
{
    public Task<bool> IsAllowedToAccessVideo(ClaimsPrincipal user, Guid videoId);
    public Task AuthorizeVideoIfNotAuthorized(Guid userId, Guid videoId);
    public Task AuthorizePlaylistIfNotAuthorized(Guid userId, Guid playlistId);
    public Task AuthorizeAuthorIfNotAuthorized(Guid userId, Guid authorId);
    public Task<bool> IsAllowedToAccessEntity(EEntityType entityType, Guid entityId, ClaimsPrincipal user);
}