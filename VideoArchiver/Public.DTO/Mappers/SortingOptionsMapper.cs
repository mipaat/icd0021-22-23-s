using App.BLL.DTO.Enums;
using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Base.Mapping;

#pragma warning disable CS1591
namespace Public.DTO.Mappers;

public class SortingOptionsMapper : BaseMapperUnidirectional<v1.EVideoSortingOptions, EVideoSortingOptions>
{
    public SortingOptionsMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddSortingOptionsMap(this AutoMapperConfig config)
    {
        config.CreateMap<v1.EVideoSortingOptions, EVideoSortingOptions>().ConvertUsingEnumMapping(opt => opt.MapByName());
        return config;
    }
}