using App.BLL.DTO.Enums;
using App.Common.Enums;
using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class PrivacyStatusMapper : BaseMapperUnidirectional<ESimplePrivacyStatus, EPrivacyStatus>
{
    public PrivacyStatusMapper(IMapper mapper) : base(mapper)
    {
    }

    public static ESimplePrivacyStatus Map(EPrivacyStatus status)
    {
        return status switch
        {
            EPrivacyStatus.Private => ESimplePrivacyStatus.Private,
            EPrivacyStatus.Unlisted => ESimplePrivacyStatus.Unlisted,
            EPrivacyStatus.Public => ESimplePrivacyStatus.Public,
            _ => ESimplePrivacyStatus.Private,
        };
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddPrivacyStatusMap(this AutoMapperConfig config)
    {
        config.CreateMap<ESimplePrivacyStatus, EPrivacyStatus>();
        return config;
    }
}