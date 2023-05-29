using System.Security.Claims;
using App.BLL.DTO.Exceptions.Identity;
using App.BLL.Identity.Services;
using App.BLL.Services;
using HeyRed.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApp.Authorization;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for methods that return files.
/// Used for file access that requires authorization.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class FileController : ControllerBase
{
    private readonly AuthorizationService _authorizationService;
    private readonly VideoPresentationService _videoPresentationService;
    private readonly TokenService _tokenService;

    /// <summary>
    /// Construct a new FileController
    /// </summary>
    /// <param name="authorizationService">Service for checking if access to a file is allowed.</param>
    /// <param name="videoPresentationService">Service for fetching relevant video presentation data (video file paths).</param>
    /// <param name="tokenService">Service for generating and validating video access tokens.</param>
    public FileController(AuthorizationService authorizationService, VideoPresentationService videoPresentationService,
        TokenService tokenService)
    {
        _authorizationService = authorizationService;
        _videoPresentationService = videoPresentationService;
        _tokenService = tokenService;
    }

    private async Task<IResult> VideoFileInternalAsync(Guid videoId, ClaimsPrincipal user)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(user, videoId))
            return Results.Forbid();

        var videoFile = await _videoPresentationService.GetVideoFileAsync(videoId);
        if (videoFile == null) return Results.NotFound();
        var filePath = videoFile.FilePath;

        var contentType = MimeTypesMap.GetMimeType(filePath);

        FileStream stream;
        try
        {
            stream = System.IO.File.OpenRead(filePath);
        }
        catch (FileNotFoundException)
        {
            return Results.NotFound();
        }
        catch (DirectoryNotFoundException)
        {
            return Results.NotFound();
        }

        return Results.File(stream, contentType, enableRangeProcessing: true);
    }

    /// <summary>
    /// Method for handling Identity Cookie authorized (and unauthorized) access to video files.
    /// </summary>
    /// <param name="videoId">The ID of the video for which to return a file.</param>
    /// <returns>FileStream of the video file</returns>
    /// <response code="200">Video file fetched successfully.</response>
    /// <response code="403">Access to video forbidden (or video doesn't exist).</response>
    /// <response code="404">Video file not found (or video doesn't exist).</response>
    [HttpGet("{videoId:guid}")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = AuthenticationSchemeDefaults.IdentityCookie)]
    public async Task<IResult> VideoFileAsync(Guid videoId)
    {
        return await VideoFileInternalAsync(videoId, User);
    }

    private const string VideoAccessTokenCookieName = "VideoAccessToken";

    /// <summary>
    /// Method for handling access token authorized (and unauthorized) access to video files.
    /// The access token is received as a cookie.
    /// If the access token is valid, a new one with a later expiration time will be set via the response.
    /// </summary>
    /// <param name="videoId">The ID of the video for which to return a file.</param>
    /// <returns>FileStream of the video file</returns>
    /// <response code="200">Video file fetched successfully.</response>
    /// <response code="403">Access to video forbidden (or video doesn't exist).</response>
    /// <response code="404">Video file not found (or video doesn't exist).</response>
    [HttpGet("{videoId:guid}")]
    public async Task<IResult> VideoFileJwtAsync(Guid videoId)
    {
        var token = HttpContext.Request.Cookies[VideoAccessTokenCookieName];
        var user = User;
        if (token != null)
        {
            try
            {
                user = await _tokenService.GetUserFromJwt(token);
                SetResponseVideoAccessToken(user);
            }
            catch (InvalidJwtException)
            {
            }
        }

        return await VideoFileInternalAsync(videoId, user);
    }

    private void SetResponseVideoAccessToken(ClaimsPrincipal user)
    {
        Response.Cookies.Append(VideoAccessTokenCookieName, _tokenService.GenerateJwt(user, 60),
            new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(1), IsEssential = true, SameSite = SameSiteMode.None, Secure = true
            });
    }

    /// <summary>
    /// Create a new video access token for the authenticated user and set it as a HTTP-only cookie in the response.
    /// This is a JWT with a short expiration time, meant for authenticating access to a video file.
    /// </summary>
    /// <response code="200">Access token set successfully.</response>
    [HttpGet]
    [Authorize(AuthenticationSchemes = AuthenticationSchemeDefaults.JwtBearer)]
    [EnableCors("CorsAllowCredentials")]
    public ActionResult<string> GetNewVideoAccessToken()
    {
        SetResponseVideoAccessToken(User);
        return Ok();
    }
}