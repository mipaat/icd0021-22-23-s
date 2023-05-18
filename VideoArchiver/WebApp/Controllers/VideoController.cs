#pragma warning disable 1591
using App.BLL;
using App.BLL.Services;
using App.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class VideoController : Controller
{
    private readonly VideoPresentationHandler _videoPresentationHandler;
    private readonly AuthorizationService _authorizationService;

    public VideoController(VideoPresentationHandler videoPresentationHandler, AuthorizationService authorizationService)
    {
        _videoPresentationHandler = videoPresentationHandler;
        _authorizationService = authorizationService;
    }

    public async Task<IActionResult> Search(EPlatform? platformQuery, string? nameQuery, string? authorQuery)
    {
        return View(new VideoSearchViewModel
        {
            PlatformQuery = platformQuery,
            NameQuery = nameQuery,
            AuthorQuery = authorQuery,
            Videos = await _videoPresentationHandler.SearchVideosAsync(platformQuery, nameQuery, authorQuery),
        });
    }

    // GET
    public async Task<IActionResult> Watch(Guid id)
    {
        if (!await _authorizationService.IsAllowedToAccessVideo(User, id)) return Forbid();
        return View(new VideoViewModel
        {
            Video = await _videoPresentationHandler.GetVideoAsync(id)
        });
    }
}