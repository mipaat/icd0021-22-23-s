using App.BLL.DTO.Mappers;
using AutoMapper;

namespace App.BLL.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        this.AddAuthorMap()
            .AddPlaylistMap()
            .AddVideoMap()
            .AddQueueItemMap()
            .AddRefreshTokenMap();
    }
}