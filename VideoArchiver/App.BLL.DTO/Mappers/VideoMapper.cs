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

    public VideoWithAuthor Map(DAL.DTO.Entities.VideoWithBasicAuthors video)
    {
        return _mapper.Map<VideoWithAuthor>(video);
    }

    public VideoWithAuthorAndComments Map(VideoWithAuthor video, ICollection<Comment> comments)
    {
        var result = _mapper.Map<VideoWithAuthorAndComments>(video);
        result.Comments = comments;
        return result;
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddVideoMap(this AutoMapperConfig config)
    {
        config.CreateMap<DAL.DTO.Entities.VideoWithBasicAuthorsAndComments, VideoWithAuthorAndComments>()
            .ForMember(v => v.Author, o =>
                o.MapFrom(v => v.Authors.First()));
        config.CreateMap<DAL.DTO.Entities.VideoWithBasicAuthors, VideoWithAuthor>()
            .ForMember(v => v.Author, o =>
                o.MapFrom(v => v.Authors.First()));
        config.CreateMap<VideoWithAuthor, VideoWithAuthorAndComments>()
            .ForMember(v => v.Comments,
                o => o.Ignore());
        return config;
    }
}