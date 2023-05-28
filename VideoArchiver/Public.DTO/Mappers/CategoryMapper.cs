using AutoMapper;
using Base.Mapping;
#pragma warning disable CS1591

namespace Public.DTO.Mappers;

public class CategoryMapper : BaseMapperUnidirectional<App.BLL.DTO.Entities.CategoryWithCreator, v1.CategoryWithCreator>
{
    public CategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddCategoryMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.BLL.DTO.Entities.CategoryWithCreator, v1.CategoryWithCreator>();
        return config;
    }
}