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
    public class CommentReplyNotificationController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public CommentReplyNotificationController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: CommentReplyNotification
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.CommentReplyNotifications.Include(c => c.Comment).Include(c => c.Receiver).Include(c => c.Reply);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: CommentReplyNotification/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.CommentReplyNotifications == null)
            {
                return NotFound();
            }

            var commentReplyNotification = await _context.CommentReplyNotifications
                .Include(c => c.Comment)
                .Include(c => c.Receiver)
                .Include(c => c.Reply)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentReplyNotification == null)
            {
                return NotFound();
            }

            return View(commentReplyNotification);
        }

        // GET: CommentReplyNotification/Create
        public IActionResult Create()
        {
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform");
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["ReplyId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform");
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
                _context.Add(commentReplyNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", commentReplyNotification.CommentId);
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", commentReplyNotification.ReceiverId);
            ViewData["ReplyId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", commentReplyNotification.ReplyId);
            return View(commentReplyNotification);
        }

        // GET: CommentReplyNotification/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.CommentReplyNotifications == null)
            {
                return NotFound();
            }

            var commentReplyNotification = await _context.CommentReplyNotifications.FindAsync(id);
            if (commentReplyNotification == null)
            {
                return NotFound();
            }
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", commentReplyNotification.CommentId);
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", commentReplyNotification.ReceiverId);
            ViewData["ReplyId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", commentReplyNotification.ReplyId);
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
                    _context.Update(commentReplyNotification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentReplyNotificationExists(commentReplyNotification.Id))
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
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", commentReplyNotification.CommentId);
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", commentReplyNotification.ReceiverId);
            ViewData["ReplyId"] = new SelectList(_context.Comments, "Id", "IdOnPlatform", commentReplyNotification.ReplyId);
            return View(commentReplyNotification);
        }

        // GET: CommentReplyNotification/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.CommentReplyNotifications == null)
            {
                return NotFound();
            }

            var commentReplyNotification = await _context.CommentReplyNotifications
                .Include(c => c.Comment)
                .Include(c => c.Receiver)
                .Include(c => c.Reply)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.CommentReplyNotifications == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.CommentReplyNotifications'  is null.");
            }
            var commentReplyNotification = await _context.CommentReplyNotifications.FindAsync(id);
            if (commentReplyNotification != null)
            {
                _context.CommentReplyNotifications.Remove(commentReplyNotification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentReplyNotificationExists(Guid id)
        {
          return (_context.CommentReplyNotifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
