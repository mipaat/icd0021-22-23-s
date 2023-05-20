using App.BLL.DTO.Entities;
using AutoMapper;
using Base.Mapping;

#pragma warning disable 1591
namespace Public.DTO.Mappers;

public class SubmissionResultMapper : BaseMapperUnidirectional<UrlSubmissionResult, v1.SubmissionResult>
{
    public SubmissionResultMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions {
    public static AutoMapperConfig AddSubmissionResultMap(this AutoMapperConfig config)
    {
        config.CreateMap<UrlSubmissionResult, v1.SubmissionResult>();
        return config;
    }
}