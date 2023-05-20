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

namespace App.BLL.Services;

public class VideoPresentationService : BaseService<VideoPresentationService>
{
    private readonly IEnumerable<IPlatformVideoPresentationHandler> _videoPresentationHandlers;

    private readonly VideoMapper _videoMapper;

    public VideoPresentationService(IEnumerable<IPlatformVideoPresentationHandler> videoPresentationHandlers, ServiceUow serviceUow, ILogger<VideoPresentationService> logger, IMapper mapper) : base(serviceUow, logger, mapper)
    {
        _videoMapper = new VideoMapper(mapper);
        _videoPresentationHandlers = videoPresentationHandlers;
    }

    public async Task<VideoWithAuthorAndComments> GetVideoAsync(Guid id)
    {
        var dalVideo = await Uow.Videos.GetByIdWithBasicAuthorsAndCommentsAsync(id);
        if (dalVideo == null) throw new VideoNotFoundInArchiveException(id);

        var video = _videoMapper.Map(dalVideo);

        foreach (var presentationHandler in _videoPresentationHandlers)
        {
            if (presentationHandler.CanHandle(video))
            {
                video = presentationHandler.Handle(video);
                break;
            }
        }

        return video;
    }

    public async Task<VideoFile?> GetVideoFileAsync(Guid id)
    {
        return (await Uow.Videos.GetVideoFilesAsync(id))?.FirstOrDefault();
    }

    public async Task<ICollection<VideoWithAuthor>> SearchVideosAsync(EPlatform? platformQuery, string? nameQuery, string? authorQuery, ICollection<Guid> categoryIds, ClaimsPrincipal user, Guid? userAuthorId)
    {
        var userId = user.GetUserIdIfExists();
        var accessAllowed = AuthorizationService.IsAllowedToAccessVideoByRole(user);
        return (await Uow.Videos.SearchVideosAsync(platformQuery, nameQuery, authorQuery, categoryIds, userId, userAuthorId, accessAllowed))
            .Select(v => _videoMapper.Map(v)).ToList();
    }
}