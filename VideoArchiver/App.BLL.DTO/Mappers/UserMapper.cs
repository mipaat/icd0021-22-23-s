using App.BLL.DTO.Entities.Identity;
using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class UserMapper : BaseMapper<App.DAL.DTO.Entities.Identity.User, User>
{
    public UserMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddUserMap(this AutoMapperConfig config)
    {
        config.CreateMap<DAL.DTO.Entities.Identity.User, User>().ReverseMap();
        return config;
    }
}