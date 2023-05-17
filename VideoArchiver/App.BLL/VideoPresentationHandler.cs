using App.BLL.DTO.Contracts;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using App.BLL.Exceptions;
using App.Common;
using App.Contracts.DAL;
using AutoMapper;

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
}