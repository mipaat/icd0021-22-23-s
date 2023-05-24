using App.BLL.DTO.Enums;
using App.BLL.DTO.Mappers;
using App.Common.Enums;
using AutoMapper;

namespace App.BLL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        this.AddAuthorMap()
            .AddVideoMap()
            .AddRefreshTokenMap()
            .AddUserMap()
            .AddQueueItemMap()
            .AddCategoryMap()
            .AddCommentMap();
    }
}