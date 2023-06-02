using App.BLL.Contracts.Services;
using App.BLL.Identity.Services;
using App.Common;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;
using Swashbuckle.AspNetCore.Annotations;
using IAuthorizationService = App.BLL.Contracts.Services.IAuthorizationService;

namespace WebApp.ApiControllers;

/// <summary>
/// API controller for accessing videos in the archive.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class VideosController : ControllerBase
{
    private readonly IVideoPresentationService _videoPresentationService;
    private readonly UserService _userService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IVideoService _videoService;
    private readonly PlatformMapper _platformMapper;
    private readonly VideoMapper _videoMapper;
    private readonly SortingOptionsMapper _sortingOptionsMapper;
    private readonly PrivacyStatusMapper _privacyStatusMapper;

    /// <summary>
    /// Construct a new VideosController.
    /// </summary>
    /// <param name="videoPresentationService">BLL service for handling video presentation.</param>
    /// <param name="mapper">Automapper for mapping BLL entities to API DTOs.</param>
    /// <param name="userService">BLL service for handling users.</param>
    /// <param name="authorizationService">BLL service for handling authorization.</param>
    /// <param name="videoService">BLL service for managing videos.</param>
    public VideosController(IVideoPresentationService videoPresentationService, IMapper mapper, UserService userService, IAuthorizationService authorizationService, IVideoService videoService)
    {
        _videoPresentationService = videoPresentationService;
        _userService = userService;
        _authorizationService = authorizationService;
        _videoService = videoService;
        _videoMapper = new VideoMapper(mapper);
        _platformMapper = new PlatformMapper(mapper);
        _sortingOptionsMapper = new SortingOptionsMapper(mapper);
        _privacyStatusMapper = new PrivacyStatusMapper(mapper);
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
            categoryIds: query.CategoryIdsQuery,
            user: User, userAuthorId: query.UserAuthorId,
            page: query.Page, limit: query.Limit,
            sortingOptions: sortingOptions, descending: query.Descending
        );
        return Ok(_videoMapper.Map(videos));
    }

    /// <summary>
    /// Fetch information about a video from the archive.
    /// </summary>
    /// <param name="id">The unique ID of the video.</param>
    /// <returns>The fetched video.</returns>
    /// <response code="200">Video fetched successfully.</response>
    /// <response code="403">Access to video not allowed or video not found.</response>
    /// <response code="404">Video not found.</response>
    [HttpGet]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<VideoWithAuthor>> GetById(Guid id)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, id)) return Forbid();
        var video = await _videoPresentationService.GetVideoWithAuthor(id);
        if (video == null) return NotFound();
        return Ok(_videoMapper.Map(video));
    }

    /// <summary>
    /// Update a video's privacy status in the archive.
    /// Requires administrator permissions.
    /// </summary>
    /// <param name="id">The unique ID of the video</param>
    /// <param name="data">The privacy status to set the video to.</param>
    /// <response code="200">Update executed successfully.
    /// NB! This is also returned if the video doesn't exist.</response>
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.AdminOrSuperAdmin)]
    public async Task<IActionResult> SetPrivacyStatus([FromQuery] Guid id, [FromBody] InternalPrivacyStatusUpdateData data)
    {
        await _videoService.SetInternalPrivacyStatus(id, _privacyStatusMapper.MapSimpleBll(data.Status));
        return Ok();
    }
}