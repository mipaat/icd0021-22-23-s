#pragma warning disable 1591
using App.BLL;
using App.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(Roles = SubmitService.AllowedToSubmitRoles)]
public class SubmitLinkController : Controller
{
    private readonly ServiceUow _serviceUow;

    public SubmitLinkController(ServiceUow serviceUow)
    {
        _serviceUow = serviceUow;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Submit(LinkSubmissionViewModel linkSubmission)
    {
        var result = await _serviceUow.SubmitService.SubmitGenericUrlAsync(linkSubmission.Link, User, linkSubmission.SubmitPlaylist);
        await _serviceUow.SaveChangesAsync();

        return RedirectToAction(nameof(Result), result);
    }

    public IActionResult Result(UrlSubmissionResultViewModel model)
    {
        return View(model);
    }
}