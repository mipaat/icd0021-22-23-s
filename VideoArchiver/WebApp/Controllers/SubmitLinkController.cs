#pragma warning disable 1591
using App.BLL;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;

namespace WebApp.Controllers;

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
    public async Task<IActionResult> Submit([Bind($"{nameof(LinkSubmission.Link)}")] LinkSubmission linkSubmission)
    {
        await _serviceUow.SubmitService.SubmitGenericUrlAsync(linkSubmission.Link, User);
        await _serviceUow.SaveChangesAsync();
        return RedirectToAction(nameof(Result));
    }

    public IActionResult Result()
    {
        return View();
    }
}