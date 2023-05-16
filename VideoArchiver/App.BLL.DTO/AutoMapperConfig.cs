using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using AutoMapper;

namespace App.BLL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        this.AddAuthorMap()
            .AddVideoMap()
            .AddRefreshTokenMap()
            .AddGameMap();
        CreateMap<DAL.DTO.Entities.Comment, Comment>();
    }
}