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
            .AddGameMap()
            .AddUserMap()
            .AddQueueItemMap()
            .AddCategoryMap();
        CreateMap<DAL.DTO.Entities.Comment, Comment>();
    }
}