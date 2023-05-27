using App.BLL.Identity.Services;
using App.BLL.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApp.ApiControllers;

/// <summary>
/// API controller for accessing videos in the archive.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class VideosController : ControllerBase
{
    private readonly VideoPresentationService _videoPresentationService;
    private readonly UserService _userService;
    private readonly PlatformMapper _platformMapper;
    private readonly VideoMapper _videoMapper;
    private readonly SortingOptionsMapper _sortingOptionsMapper;

    /// <summary>
    /// Construct a new VideosController.
    /// </summary>
    /// <param name="videoPresentationService">BLL service for handling video presentation.</param>
    /// <param name="mapper">Automapper for mapping BLL entities to API DTOs.</param>
    /// <param name="userService">BLL service for handling users.</param>
    public VideosController(VideoPresentationService videoPresentationService, IMapper mapper, UserService userService)
    {
        _videoPresentationService = videoPresentationService;
        _userService = userService;
        _videoMapper = new VideoMapper(mapper);
        _platformMapper = new PlatformMapper(mapper);
        _sortingOptionsMapper = new SortingOptionsMapper(mapper);
    }

    /// <summary>
    /// Search videos in the archive.
    /// </summary>
    /// <param name="query">Filtering, sorting, and pagination options for the query.</param>
    /// <returns>List of videos.</returns>
    /// <response code="200">Videos fetched successfully.</response>
    /// <response code="400">Invalid author ID specified - author isn't a sub-author of authenticated user.</response>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerResponse(StatusCodes.Status200OK, null, typeof(VideoSearchResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(RestApiErrorResponse))]
    public async Task<ActionResult<VideoSearchResult>> Search([FromQuery] VideoSearchQuery query)
    {
        App.Common.Enums.EPlatform? platformQuery =
            query.PlatformQuery == null ? null : _platformMapper.Map(query.PlatformQuery.Value);
        var sortingOptions = _sortingOptionsMapper.Map(query.SortingOptions);
        if (query.UserAuthorId != null && !await _userService.IsUserSubAuthor(query.UserAuthorId.Value, User))
        {
            return BadRequest(new RestApiErrorResponse
            {
                Error = $"Author {query.UserAuthorId.Value} isn't a sub-author of user {User}",
                ErrorType = EErrorType.UserAuthorMismatch,
            });
        }

        var videos = await _videoPresentationService.SearchVideosAsync(
            platformQuery: platformQuery, nameQuery: query.NameQuery, authorQuery: query.AuthorQuery,
            categoryIds: query.CategoryIdsQuery ?? new List<Guid>(),
            user: User, userAuthorId: query.UserAuthorId,
            page: query.Page, limit: query.Limit,
            sortingOptions: sortingOptions, descending: query.Descending
        );
        return Ok(_videoMapper.Map(videos));
    }
}