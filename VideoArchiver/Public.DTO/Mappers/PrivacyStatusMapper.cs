using AutoMapper;
using Base.Mapping;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class PrivacyStatusMapper : BaseMapper<App.Common.Enums.EPrivacyStatus, v1.EPrivacyStatus>
{
    public PrivacyStatusMapper(IMapper mapper) : base(mapper)
    {
    }

    public v1.ESimplePrivacyStatus MapSimple(App.Common.Enums.EPrivacyStatus privacyStatus)
    {
        return Mapper.Map<v1.ESimplePrivacyStatus>(privacyStatus);
    }

    public App.Common.Enums.EPrivacyStatus MapSimple(v1.ESimplePrivacyStatus privacyStatus)
    {
        return Mapper.Map<App.Common.Enums.EPrivacyStatus>(privacyStatus);
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddPrivacyStatusMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.Common.Enums.EPrivacyStatus, v1.EPrivacyStatus>().ReverseMap();
        config.CreateMap<App.Common.Enums.EPrivacyStatus, v1.ESimplePrivacyStatus>().ReverseMap();
        return config;
    }
}