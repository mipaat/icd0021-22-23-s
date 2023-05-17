using App.BLL.DTO.Entities;
using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class AuthorMapper : BaseMapperUnidirectional<App.DAL.DTO.Entities.Author, Author>
{
    public AuthorMapper(IMapper mapper) : base(mapper)
    {
    }

    public Author MapBasic(App.DAL.DTO.Entities.AuthorBasic author)
    {
        return Mapper.Map<Author>(author);
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddAuthorMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.Author, Author>();
        config.CreateMap<DAL.DTO.Entities.Author, Author>();
        return config;
    }
}