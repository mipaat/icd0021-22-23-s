using AutoMapper;
using Base.Mapping;
using Public.DTO.v1.Identity;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class UserMapper : BaseMapperUnidirectional<App.BLL.DTO.Entities.Identity.User, User>
{
    public UserMapper(IMapper mapper) : base(mapper)
    {
    }

    public UserWithRoles Map(App.BLL.DTO.Entities.Identity.UserWithRoles user)
    {
        return Mapper.Map<UserWithRoles>(user);
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddUserMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.BLL.DTO.Entities.Identity.User, User>();
        config.CreateMap<App.BLL.DTO.Entities.Identity.UserWithRoles, UserWithRoles>();
        return config;
    }
}