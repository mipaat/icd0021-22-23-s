using System.Security.Claims;
using App.BLL.DTO.Contracts;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using App.BLL.Exceptions;
using App.BLL.Services;
using App.Common;
using App.Common.Enums;
using App.Contracts.DAL;
using AutoMapper;
using Base.WebHelpers;

namespace App.BLL;

public class VideoPresentationHandler
{
    private readonly IEnumerable<IPlatformVideoPresentationHandler> _videoPresentationHandlers;
    private readonly IAppUnitOfWork _uow;
    private readonly VideoMapper _videoMapper;

    public VideoPresentationHandler(IAppUnitOfWork uow, IEnumerable<IPlatformVideoPresentationHandler> videoPresentationHandlers, IMapper mapper)
    {
        _videoPresentationHandlers = videoPresentationHandlers;
        _uow = uow;
        _videoMapper = new VideoMapper(mapper);
    }

    public async Task<VideoWithAuthorAndComments> GetVideoAsync(Guid id)
    {
        var dalVideo = await _uow.Videos.GetByIdWithBasicAuthorsAndCommentsAsync(id);
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
        return (await _uow.Videos.GetVideoFilesAsync(id))?.FirstOrDefault();
    }

    public async Task<ICollection<VideoWithAuthor>> SearchVideosAsync(EPlatform? platformQuery, string? nameQuery, string? authorQuery, ICollection<Guid> categoryIds, ClaimsPrincipal user, Guid? userAuthorId)
    {
        var userId = user.GetUserIdIfExists();
        var accessAllowed = AuthorizationService.IsAllowedToAccessVideoByRole(user);
        return (await _uow.Videos.SearchVideosAsync(platformQuery, nameQuery, authorQuery, categoryIds, userId, userAuthorId, accessAllowed))
            .Select(v => _videoMapper.Map(v)).ToList();
    }
}