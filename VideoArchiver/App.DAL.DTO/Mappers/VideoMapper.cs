using App.DAL.DTO.Entities;
using AutoMapper;
using Base.Mapping;

namespace App.DAL.DTO.Mappers;

public class VideoMapper : BaseMapper<App.Domain.Video, Video>
{
    public VideoMapper(IMapper mapper) : base(mapper)
    {
    }

    public VideoWithComments? MapWithComments(App.Domain.Video? video)
    {
        if (video == null) return null;
        return Mapper.Map<App.Domain.Video, VideoWithComments>(video);
    }
}