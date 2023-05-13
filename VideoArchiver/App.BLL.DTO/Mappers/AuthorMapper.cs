using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class AuthorMapper : BaseMapperUnidirectional<App.DAL.DTO.Entities.Author, Entities.Author>
{
    public AuthorMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static class AuthorMapperExtensions
{
    public static AutoMapperConfig AddAuthorMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.Author, Entities.Author>();
        return config;
    }
}