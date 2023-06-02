#pragma warning disable 1591
using App.BLL.Contracts;
using App.BLL.DTO.Entities;
using App.BLL.Exceptions;
using App.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize(Roles = SubmitService.AllowedToSubmitRoles)]
public class SubmitLinkController : Controller
{
    private readonly IServiceUow _serviceUow;

    public SubmitLinkController(IServiceUow serviceUow)
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
        UrlSubmissionResult result;
        try
        {
            result = await _serviceUow.SubmitService.SubmitGenericUrlAsync(linkSubmission.Link, User,
                linkSubmission.SubmitPlaylist);
        }
        catch (UnrecognizedUrlException)
        {
            return RedirectToAction(nameof(UnrecognizedUrl), new { Url = linkSubmission.Link });
        }

        await _serviceUow.SaveChangesAsync();

        return RedirectToAction(nameof(Result), result);
    }

    [HttpGet]
    public IActionResult UnrecognizedUrl(string url)
    {
        return View(url);
    }

    public IActionResult Result(UrlSubmissionResultViewModel model)
    {
        return View(model);
    }
}