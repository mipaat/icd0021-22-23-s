using App.BLL;
using App.BLL.Services;
using HeyRed.Mime;
using Microsoft.AspNetCore.Authorization;
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
    private readonly VideoPresentationHandler _videoPresentationHandler;

    /// <summary>
    /// Construct a new FileController
    /// </summary>
    /// <param name="authorizationService">Service for checking if access to a file is allowed.</param>
    /// <param name="videoPresentationHandler">Service for fetching relevant video presentation data (video file paths).</param>
    public FileController(AuthorizationService authorizationService, VideoPresentationHandler videoPresentationHandler)
    {
        _authorizationService = authorizationService;
        _videoPresentationHandler = videoPresentationHandler;
    }

    /// <summary>
    /// Method for handling authorized (and unauthorized) access to video files.
    /// </summary>
    /// <param name="videoId">The ID of the video for which to return a file.</param>
    /// <returns>FileStream of the video file</returns>
    /// <response code="200">Video file fetched successfully.</response>
    /// <response code="403">Access to video forbidden (or video doesn't exist).</response>
    /// <response code="404">Video file not found (or video doesn't exist).</response>
    [HttpGet("VideoFile/{videoId:guid}")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = AuthenticationSchemeDefaults.IdentityAndJwt)]
    public async Task<IResult> VideoFileAsync(Guid videoId)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, videoId))
            return Results.Forbid();

        var videoFile = await _videoPresentationHandler.GetVideoFileAsync(videoId);
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
}