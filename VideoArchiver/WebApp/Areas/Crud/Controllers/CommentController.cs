using App.Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class CommentController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public CommentController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Comments.GetAllAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.GetByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        private async Task SetupViewData(Comment? comment = null)
        {
            var comments = await _uow.Comments.GetAllAsync();
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", comment?.AuthorId);
            ViewData["ConversationRootId"] = new SelectList(comments, "Id", "IdOnPlatform", comment?.ConversationRootId);
            ViewData["ReplyTargetId"] = new SelectList(comments, "Id", "IdOnPlatform", comment?.ReplyTargetId);
            ViewData["VideoId"] = new SelectList(await _uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", comment?.VideoId);
        }

        // GET: Comment/Create
        public async Task<IActionResult> Create()
        {
            await SetupViewData();
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Platform,IdOnPlatform,AuthorId,VideoId,ReplyTargetId,ConversationRootId,Content,LikeCount,DislikeCount,ReplyCount,CreatedAt,CreatedAtVideoTimecode,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,FetchSuccess,AddedToArchiveAt,Id")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Id = Guid.NewGuid();
                _uow.Comments.Add(comment);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(comment);
            return View(comment);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.GetByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            await SetupViewData(comment);
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Platform,IdOnPlatform,AuthorId,VideoId,ReplyTargetId,ConversationRootId,Content,LikeCount,DislikeCount,ReplyCount,CreatedAt,CreatedAtVideoTimecode,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,FetchSuccess,AddedToArchiveAt,Id")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Comments.Update(comment);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Comments.ExistsAsync(comment.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(comment);
            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.GetByIdAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Comments.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
