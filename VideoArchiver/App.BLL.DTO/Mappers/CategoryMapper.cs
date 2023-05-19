using App.BLL.DTO.Entities;
using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class CategoryMapper : BaseMapper<App.DAL.DTO.Entities.CategoryWithCreator, CategoryWithCreator>
{
    public CategoryMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddCategoryMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.CategoryWithCreator, CategoryWithCreator>()
            .ReverseMap().ForMember(e => e.CreatorId, o =>
                o.MapFrom(e => e.Creator!.Id));
        return config;
    }
}