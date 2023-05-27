using AutoMapper;
using Base.Mapping;

#pragma warning disable CS1591
namespace Public.DTO.Mappers;

public class ImageFileMapper : BaseMapperUnidirectional<App.Common.ImageFile, v1.ImageFile>
{
    public ImageFileMapper(IMapper mapper) : base(mapper)
    {
    }
}

public static partial class AutoMapperConfigExtensions
{
    public static AutoMapperConfig AddImageFileMap(this AutoMapperConfig config, Func<string> getBasePath)
    {
        config.CreateMap<App.Common.ImageFile, v1.ImageFile>()
            .ForMember(i => i.Url,
                o => o.MapFrom(
                    i => i.LocalFilePath != null ? $"{getBasePath()}/{i.LocalFilePath}" : i.Url));
        return config;
    }
}