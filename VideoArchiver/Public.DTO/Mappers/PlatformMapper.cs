using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Base.Mapping;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class PlatformMapper : BaseMapper<App.Common.Enums.EPlatform, v1.EPlatform>
{
    public PlatformMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddPlatformMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.Common.Enums.EPlatform, v1.EPlatform>()
            .ConvertUsingEnumMapping(opt => opt.MapByName())
            .ReverseMap();
        return config;
    }
}