using App.BLL.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.Mappers;
using Public.DTO.v1;

namespace WebApp.ApiControllers;

/// <summary>
/// Api controller for accessing comments.
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class CommentController : ControllerBase
{
    private readonly AuthorizationService _authorizationService;
    private readonly CommentService _commentService;
    private readonly CommentMapper _commentMapper;

    /// <summary>
    /// Construct a new CommentController.
    /// </summary>
    /// <param name="authorizationService">BLL service for handling authorization.</param>
    /// <param name="commentService">BLL service for handling comments.</param>
    /// <param name="mapper">Automapper for mapping BLL entities to API DTOs.</param>
    public CommentController(AuthorizationService authorizationService, CommentService commentService, IMapper mapper)
    {
        _authorizationService = authorizationService;
        _commentService = commentService;
        _commentMapper = new CommentMapper(mapper);
    }

    /// <summary>
    /// Fetch archived comments for a video.
    /// </summary>
    /// <param name="videoId">The ID of the video to fetch comments for.</param>
    /// <param name="limit">The amount of comments to fetch. Must be between 1 and 100 (inclusive).</param>
    /// <param name="page">The page of comments to fetch (amount of skipped comments depends on limit).</param>
    /// <returns>List of comments.</returns>
    /// <response code="200">Comments fetched successfully.</response>
    /// <response code="403">Access to video and its comments forbidden.</response>
    [HttpGet]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<List<Comment>>> GetVideoComments(Guid videoId, int limit, int page)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, videoId)) return Forbid();
        var comments = await _commentService.GetVideoComments(videoId, limit, page, null);
        return Ok(comments.Select(c => _commentMapper.Map(c)!).ToList());
    }
}