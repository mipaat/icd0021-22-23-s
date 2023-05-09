#pragma warning disable 1591
using App.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;

namespace WebApp.Controllers;

public class SubmitLinkController : Controller
{
    private readonly IAppUnitOfWork _uow;
    private readonly UrlSubmissionHandler _urlSubmissionHandler;

    public SubmitLinkController(IAppUnitOfWork uow, UrlSubmissionHandler urlSubmissionHandler)
    {
        _uow = uow;
        _urlSubmissionHandler = urlSubmissionHandler;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Submit([Bind($"{nameof(LinkSubmission.Link)}")] LinkSubmission linkSubmission)
    {
        await _urlSubmissionHandler.SubmitGenericUrlAsync(linkSubmission.Link, User);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Result));
    }

    public IActionResult Result()
    {
        return View();
    }
}