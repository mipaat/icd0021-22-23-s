using AutoMapper;
using Base.Mapping;

#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class VideoMapper : BaseMapperUnidirectional<App.BLL.DTO.Entities.BasicVideoWithAuthor, v1.BasicVideoWithAuthor>
{
    public VideoMapper(IMapper mapper) : base(mapper)
    {
    }

    public v1.VideoSearchResult Map(IEnumerable<App.BLL.DTO.Entities.BasicVideoWithAuthor> videos)
    {
        return new v1.VideoSearchResult
        {
            Videos = videos.Select(v => Map(v)!).ToList()
        };
    }

    public v1.VideoWithAuthor Map(App.BLL.DTO.Entities.VideoWithAuthor video)
    {
        return Mapper.Map<v1.VideoWithAuthor>(video);
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddVideoMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.BLL.DTO.Entities.BasicVideoWithAuthor, v1.BasicVideoWithAuthor>();
        config.CreateMap<App.BLL.DTO.Entities.VideoWithAuthor, v1.VideoWithAuthor>();
        return config;
    }
}