#pragma warning disable 1591
using App.BLL;
using App.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;

namespace WebApp.Controllers;

public class SubmitLinkController : Controller
{
    private readonly SubmitService _submitService;

    public SubmitLinkController(SubmitService submitService)
    {
        _submitService = submitService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Submit([Bind($"{nameof(LinkSubmission.Link)}")] LinkSubmission linkSubmission)
    {
        await _submitService.SubmitGenericUrlAsync(linkSubmission.Link, User);
        await _submitService.SaveChangesAsync();
        return RedirectToAction(nameof(Result));
    }

    public IActionResult Result()
    {
        return View();
    }
}