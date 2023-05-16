using App.BLL.DTO.Entities;
using AutoMapper;

namespace App.BLL.DTO.Mappers;

public class VideoMapper
{
    private readonly IMapper _mapper;

    public VideoMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public VideoWithAuthorAndComments Map(DAL.DTO.Entities.VideoWithBasicAuthorsAndComments video)
    {
        return _mapper.Map<VideoWithAuthorAndComments>(video);
    }
}

public static class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddVideoMap(this AutoMapperConfig config)
    {
        config.CreateMap<DAL.DTO.Entities.VideoWithBasicAuthorsAndComments, VideoWithAuthorAndComments>()
            .ForMember(v => v.Author, o =>
                o.MapFrom(v => v.Authors.First()));
        return config;
    }
}