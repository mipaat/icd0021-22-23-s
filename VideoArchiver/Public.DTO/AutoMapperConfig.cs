using App.Common.Enums;
using AutoMapper;
using Public.DTO.Mappers;

namespace Public.DTO;

/// <summary>
/// Configuration class for configuring an automapper between public DTOs and internal DTOs.
/// </summary>
public class AutoMapperConfig : Profile
{
    /// <summary>
    /// Constructor for creating a new instance of this automapper configuration.
    /// </summary>
    public AutoMapperConfig()
    {
        CreateMap<EPlatform, v1.EPlatform>();
        CreateMap<EEntityType, v1.EEntityType>();
        CreateMap<App.BLL.DTO.Entities.Identity.Role, v1.Identity.Role>();

        this.AddSubmissionResultMap()
            .AddUserAuthorMap()
            .AddUserMap()
            .AddQueueItemMap();
    }
}