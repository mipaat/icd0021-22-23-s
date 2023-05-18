using App.BLL;
using App.BLL.DTO;
using Base.WebHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = RoleNames.Admin)]
public class QueueItemsApprovalController : Controller
{
    private readonly ServiceUow ServiceUow;

    public QueueItemsApprovalController(ServiceUow serviceUow)
    {
        ServiceUow = serviceUow;
    }

    public async Task<ActionResult> Index()
    {
        return View(new QueueItemsApprovalViewModel
        {
            QueueItems = await ServiceUow.QueueItemService.GetAllAwaitingApprovalAsync(),
        });
    }

    public async Task<IActionResult> ApproveQueueItem([FromForm] Guid id, [FromForm] bool grantAccess = true)
    {
        await ServiceUow.QueueItemService.ApproveAsync(id, User.GetUserId(), grantAccess);
        await ServiceUow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteQueueItem([FromForm] Guid id)
    {
        await ServiceUow.QueueItemService.DeleteAsync(id);
        await ServiceUow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}