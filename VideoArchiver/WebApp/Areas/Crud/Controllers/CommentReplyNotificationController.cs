using App.Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class CommentReplyNotificationController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public CommentReplyNotificationController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: CommentReplyNotification
        public async Task<IActionResult> Index()
        {
            return View(await _uow.CommentReplyNotifications.GetAllAsync());
        }

        // GET: CommentReplyNotification/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentReplyNotification = await _uow.CommentReplyNotifications.GetByIdAsync(id.Value);
            if (commentReplyNotification == null)
            {
                return NotFound();
            }

            return View(commentReplyNotification);
        }

        private async Task SetupViewData(CommentReplyNotification? commentReplyNotification = null)
        {
            var comments = await _uow.Comments.GetAllAsync();
            ViewData["CommentId"] = new SelectList(comments, "Id", "IdOnPlatform", commentReplyNotification?.CommentId);
            ViewData["ReceiverId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", commentReplyNotification?.ReceiverId);
            ViewData["ReplyId"] = new SelectList(comments, "Id", "IdOnPlatform", commentReplyNotification?.ReplyId);
        }

        // GET: CommentReplyNotification/Create
        public async Task<IActionResult> Create()
        {
            await SetupViewData();
            return View();
        }

        // POST: CommentReplyNotification/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReplyId,CommentId,ReceiverId,SentAt,DeliveredAt,Id")] CommentReplyNotification commentReplyNotification)
        {
            if (ModelState.IsValid)
            {
                commentReplyNotification.Id = Guid.NewGuid();
                _uow.CommentReplyNotifications.Add(commentReplyNotification);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(commentReplyNotification);
            return View(commentReplyNotification);
        }

        // GET: CommentReplyNotification/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentReplyNotification = await _uow.CommentReplyNotifications.GetByIdAsync(id.Value);
            if (commentReplyNotification == null)
            {
                return NotFound();
            }

            await SetupViewData(commentReplyNotification);
            return View(commentReplyNotification);
        }

        // POST: CommentReplyNotification/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReplyId,CommentId,ReceiverId,SentAt,DeliveredAt,Id")] CommentReplyNotification commentReplyNotification)
        {
            if (id != commentReplyNotification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.CommentReplyNotifications.Update(commentReplyNotification);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.CommentReplyNotifications.ExistsAsync(commentReplyNotification.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(commentReplyNotification);
            return View(commentReplyNotification);
        }

        // GET: CommentReplyNotification/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentReplyNotification = await _uow.CommentReplyNotifications.GetByIdAsync(id.Value);
            if (commentReplyNotification == null)
            {
                return NotFound();
            }

            return View(commentReplyNotification);
        }

        // POST: CommentReplyNotification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.CommentReplyNotifications.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
