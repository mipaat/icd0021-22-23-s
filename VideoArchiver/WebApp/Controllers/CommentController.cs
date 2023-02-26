using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public CommentController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.Comments.Include(c => c.Author).Include(c => c.ConversationRoot).Include(c => c.ReplyTarget).Include(c => c.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.ConversationRoot)
                .Include(c => c.ReplyTarget)
                .Include(c => c.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comment/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["ConversationRootId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform");
            ViewData["ReplyTargetId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
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
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", comment.AuthorId);
            ViewData["ConversationRootId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", comment.ConversationRootId);
            ViewData["ReplyTargetId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", comment.ReplyTargetId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", comment.VideoId);
            return View(comment);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", comment.AuthorId);
            ViewData["ConversationRootId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", comment.ConversationRootId);
            ViewData["ReplyTargetId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", comment.ReplyTargetId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", comment.VideoId);
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
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", comment.AuthorId);
            ViewData["ConversationRootId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", comment.ConversationRootId);
            ViewData["ReplyTargetId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", comment.ReplyTargetId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", comment.VideoId);
            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.ConversationRoot)
                .Include(c => c.ReplyTarget)
                .Include(c => c.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Comments == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(Guid id)
        {
          return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
