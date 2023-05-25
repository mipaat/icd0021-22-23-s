using AutoMapper;
using Base.Mapping;
using Public.DTO.v1.Identity;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class UserAuthorMapper : BaseMapperUnidirectional<App.BLL.DTO.Entities.Author, UserSubAuthor>
{
    public UserAuthorMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddUserAuthorMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.BLL.DTO.Entities.Author, UserSubAuthor>();
        return config;
    }
}