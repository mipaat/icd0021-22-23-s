using App.BLL.Base;
using App.BLL.DTO.Enums;
using App.BLL.DTO.Mappers;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class VideoService : BaseService<VideoService>
{
    private readonly PrivacyStatusMapper _privacyStatusMapper;
    
    public VideoService(ServiceUow serviceUow, ILogger<VideoService> logger, IMapper mapper) : base(serviceUow, logger, mapper)
    {
        _privacyStatusMapper = new PrivacyStatusMapper(mapper);
    }

    public async Task SetInternalPrivacyStatus(Guid videoId, ESimplePrivacyStatus status)
    {
        await Uow.Videos.SetInternalPrivacyStatus(videoId, _privacyStatusMapper.Map(status));
    }
}