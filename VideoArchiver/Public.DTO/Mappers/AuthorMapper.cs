using AutoMapper;
using Base.Mapping;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class AuthorMapper : BaseMapperUnidirectional<App.BLL.DTO.Entities.Author, v1.Author>
{
    public AuthorMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddAuthorMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.BLL.DTO.Entities.Author, v1.Author>();
        return config;
    }
}