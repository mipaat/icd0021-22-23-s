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
    private readonly VideoPresentationService _videoPresentationService;
    private readonly AuthorService _authorService;

    /// <summary>
    /// Construct a new FileController
    /// </summary>
    /// <param name="authorizationService">Service for checking if access to a file is allowed.</param>
    /// <param name="videoPresentationService">Service for fetching relevant video presentation data (video file paths).</param>
    /// <param name="authorService">Service for fetching author profile image file paths.</param>
    public FileController(AuthorizationService authorizationService, VideoPresentationService videoPresentationService, AuthorService authorService)
    {
        _authorizationService = authorizationService;
        _videoPresentationService = videoPresentationService;
        _authorService = authorService;
    }

    /// <summary>
    /// Method for handling authorized (and unauthorized) access to video files.
    /// </summary>
    /// <param name="videoId">The ID of the video for which to return a file.</param>
    /// <returns>FileStream of the video file</returns>
    /// <response code="200">Video file fetched successfully.</response>
    /// <response code="403">Access to video forbidden (or video doesn't exist).</response>
    /// <response code="404">Video file not found (or video doesn't exist).</response>
    [HttpGet("{videoId:guid}")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = AuthenticationSchemeDefaults.IdentityAndJwt)]
    public async Task<IResult> VideoFileAsync(Guid videoId)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, videoId))
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
    /// Method for access to author profile image files.
    /// Allows unauthorized access.
    /// </summary>
    /// <param name="authorId">The archive ID of the author whose profile image is requested.</param>
    /// <param name="large">Boolean, whether or not the returned image should be large. Will work inconsistently or not at all.</param>
    /// <returns>FileStream of the image file.</returns>
    /// <response code="200">Profile image file fetched successfully.</response>
    /// <response code="404">Author, profile image, or file not found.</response>
    [HttpGet("{authorId:guid}")]
    public async Task<IResult> AuthorProfileImageAsync(Guid authorId, bool large = false)
    {
        var imageFile = await _authorService.GetProfileImageAsync(authorId, large);
        if (imageFile?.LocalFilePath == null) return Results.NotFound();
        var filePath = imageFile.LocalFilePath;

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

        return Results.File(stream, contentType);
    }
}