using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class VideoMapper : BaseMapperUnidirectional<App.DAL.DTO.Entities.Video, Entities.Video>
{
    public VideoMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static class VideoMapperExtensions
{
    public static AutoMapperConfig AddVideoMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.Video, Entities.Video>();
        return config;
    }
}