using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class RefreshTokenMapper : BaseMapper<App.DAL.DTO.Entities.Identity.RefreshToken, Entities.Identity.RefreshToken>
{
    public RefreshTokenMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static class RefreshTokenMapperExtensions
{
    public static AutoMapperConfig AddRefreshTokenMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.Identity.RefreshToken, Entities.Identity.RefreshToken>()
            .ReverseMap();
        return config;
    }
}