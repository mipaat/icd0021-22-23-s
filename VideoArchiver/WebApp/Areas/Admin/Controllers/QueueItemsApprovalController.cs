using App.BLL;
using App.Common;
using Base.WebHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.ViewModels;
#pragma warning disable CS1591

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = RoleNames.AdminOrSuperAdmin)]
public class QueueItemsApprovalController : Controller
{
    private readonly ServiceUow _serviceUow;

    public QueueItemsApprovalController(ServiceUow serviceUow)
    {
        _serviceUow = serviceUow;
    }

    public async Task<ActionResult> Index()
    {
        return View(new QueueItemsApprovalViewModel
        {
            QueueItems = await _serviceUow.QueueItemService.GetAllAwaitingApprovalAsync(),
        });
    }

    [HttpPost]
    public async Task<IActionResult> ApproveQueueItem([FromForm] Guid id, [FromForm] bool grantAccess = true)
    {
        await _serviceUow.QueueItemService.ApproveAsync(id, User.GetUserId(), grantAccess);
        await _serviceUow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteQueueItem([FromForm] Guid id)
    {
        await _serviceUow.QueueItemService.DeleteAsync(id);
        await _serviceUow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}