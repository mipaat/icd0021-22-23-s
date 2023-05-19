using App.BLL.DTO.Entities;
using AutoMapper;
using Base.Mapping;

namespace App.BLL.DTO.Mappers;

public class CategoryMapper : BaseMapper<App.DAL.DTO.Entities.CategoryWithCreator, CategoryWithCreator>
{
    public CategoryMapper(IMapper mapper) : base(mapper)
    {
    }

    public App.DAL.DTO.Entities.CategoryWithCreator Map(CategoryData category, Guid? id = null)
    {
        var entity = Mapper.Map<App.DAL.DTO.Entities.CategoryWithCreator>(category);
        if (id != null) entity.Id = id.Value;
        return entity;
    }

    public App.DAL.DTO.Entities.CategoryWithCreator Map(CategoryDataWithCreatorId category, Guid? id = null)
    {
        var entity = Mapper.Map<App.DAL.DTO.Entities.CategoryWithCreator>(category);
        if (id != null) entity.Id = id.Value;
        return entity;
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddCategoryMap(this AutoMapperConfig config)
    {
        config.CreateMap<App.DAL.DTO.Entities.CategoryWithCreator, CategoryWithCreator>()
            .ReverseMap().ForMember(e => e.CreatorId, o =>
                o.MapFrom(e => e.Creator!.Id));
        config.CreateMap<CategoryData, App.DAL.DTO.Entities.CategoryWithCreator>();
        config.CreateMap<CategoryDataWithCreatorId, App.DAL.DTO.Entities.CategoryWithCreator>();
        return config;
    }
}