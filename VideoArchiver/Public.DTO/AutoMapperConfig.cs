using App.Common.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Public.DTO.Mappers;

namespace Public.DTO;

/// <summary>
/// Configuration class for configuring an automapper between public DTOs and internal DTOs.
/// </summary>
public class AutoMapperConfig : Profile
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    /// <summary>
    /// Constructor for creating a new instance of this automapper configuration.
    /// </summary>
    public AutoMapperConfig(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        CreateMap<EEntityType, v1.EEntityType>();
        CreateMap<App.BLL.DTO.Entities.Identity.Role, v1.Identity.Role>();

        this.AddSubmissionResultMap()
            .AddUserAuthorMap()
            .AddUserMap()
            .AddQueueItemMap()
            .AddPlatformMap()
            .AddSortingOptionsMap()
            .AddVideoMap()
            .AddAuthorMap()
            .AddImageFileMap(GetWebsiteUrl)
            .AddPrivacyStatusMap()
            .AddLangStringMap()
            .AddCategoryMap()
            .AddCommentMap();
    }

    private string GetWebsiteUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null) throw new ApplicationException($"Can't get website base URL, {nameof(HttpContext)} is null");
        return $"{request.Scheme}://{request.Host}{request.PathBase}";
    }
}