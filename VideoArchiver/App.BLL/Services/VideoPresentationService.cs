using System.Security.Claims;
using App.BLL.Base;
using App.BLL.DTO.Contracts;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using App.BLL.Exceptions;
using App.Common;
using App.Common.Enums;
using AutoMapper;
using Base.WebHelpers;
using Microsoft.Extensions.Logging;
using Utils;

namespace App.BLL.Services;

public class VideoPresentationService : BaseService<VideoPresentationService>
{
    private readonly IEnumerable<IPlatformVideoPresentationHandler> _videoPresentationHandlers;

    private readonly VideoMapper _videoMapper;

    public VideoPresentationService(IEnumerable<IPlatformVideoPresentationHandler> videoPresentationHandlers,
        ServiceUow serviceUow, ILogger<VideoPresentationService> logger, IMapper mapper) : base(serviceUow, logger,
        mapper)
    {
        _videoMapper = new VideoMapper(mapper);
        _videoPresentationHandlers = videoPresentationHandlers;
    }

    public async Task<VideoWithAuthorAndComments> GetVideoAsync(Guid id, int limit, int page)
    {
        var dalVideo = await Uow.Videos.GetByIdWithBasicAuthorsAsync(id);
        if (dalVideo == null) throw new VideoNotFoundInArchiveException(id);

        var video = await ServiceUow.CommentService.LoadVideoComments(_videoMapper.Map(dalVideo), limit, page);

        foreach (var presentationHandler in _videoPresentationHandlers)
        {
            if (!presentationHandler.CanHandle(video)) continue;
            presentationHandler.Handle(video);
            break;
        }

        return video;
    }

    public async Task<VideoFile?> GetVideoFileAsync(Guid id)
    {
        return (await Uow.Videos.GetVideoFilesAsync(id))?.FirstOrDefault();
    }

    public async Task<ICollection<BasicVideoWithAuthor>> SearchVideosAsync(
        EPlatform? platformQuery, string? nameQuery, string? authorQuery, ICollection<Guid> categoryIds,
        ClaimsPrincipal user, Guid? userAuthorId,
        int page, int limit, EVideoSortingOptions sortingOptions, bool descending)
    {
        var userId = user.GetUserIdIfExists();
        var accessAllowed = AuthorizationService.IsAllowedToAccessVideoByRole(user);
        int? total = null;
        PaginationUtils.ConformValues(ref total, ref limit, ref page);
        var skipAmount = PaginationUtils.PageToSkipAmount(limit, page);
        var videos = (await Uow.Videos.SearchVideosAsync(platform: platformQuery, name: nameQuery, author: authorQuery,
                categoryIds: categoryIds,
                userAuthorId: userAuthorId, userid: userId, accessAllowed: accessAllowed,
                skipAmount: skipAmount, limit: limit, sortingOptions: sortingOptions, descending: descending))
            .Select(v => _videoMapper.Map(v)).ToList();
        foreach (var video in videos)
        {
            foreach (var handler in _videoPresentationHandlers)
            {
                if (!handler.CanHandle(video)) continue;
                handler.Handle(video);
                break;
            }
        }

        return videos;
    }
}