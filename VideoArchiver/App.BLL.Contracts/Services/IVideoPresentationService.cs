using System.Security.Claims;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Enums;
using App.Common;
using App.Common.Enums;

namespace App.BLL.Contracts.Services;

public interface IVideoPresentationService
{
    public Task<VideoWithAuthorAndComments> GetVideoWithAuthorAndCommentsAsync(Guid id, int limit, int page);
    public Task<VideoWithAuthor?> GetVideoWithAuthor(Guid id);
    public Task<VideoFile?> GetVideoFileAsync(Guid id);

    public Task<List<BasicVideoWithAuthor>> SearchVideosAsync(
        EPlatform? platformQuery, string? nameQuery, string? authorQuery, ICollection<Guid>? categoryIds,
        ClaimsPrincipal user, Guid? userAuthorId,
        int page, int limit, EVideoSortingOptions sortingOptions, bool descending);
    
}