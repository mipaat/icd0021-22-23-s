using AutoMapper;
using Base.Mapping;

#pragma warning disable CS1591
namespace Public.DTO.Mappers;

public class LangStringMapper : BaseMapper<App.Common.LangString, v1.LangString>
{
    public LangStringMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddLangStringMap(this AutoMapperConfig config)
    {
        // Looks like CreateMap between two classes inheriting from Dictionary<string, string> isn't necessary.
        // In fact, that breaks everything due to the mapper attempting to map indexers?
        return config;
    }
}