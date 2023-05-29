using AutoMapper;
using Base.Mapping;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class CommentMapper : BaseMapperUnidirectional<App.BLL.DTO.Entities.Comment, v1.Comment>
{
    public CommentMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddCommentMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.BLL.DTO.Entities.Comment, v1.Comment>();
        return config;
    }
}