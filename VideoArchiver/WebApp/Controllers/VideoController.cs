#pragma warning disable 1591
using App.BLL;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class VideoController : Controller
{
    private readonly VideoPresentationHandler _videoPresentationHandler;

    public VideoController(VideoPresentationHandler videoPresentationHandler)
    {
        _videoPresentationHandler = videoPresentationHandler;
    }

    // GET
    public async Task<IActionResult> Watch(Guid id)
    {
        return View(new VideoViewModel
        {
            Video = await _videoPresentationHandler.GetVideoAsync(id)
        });
    }
}