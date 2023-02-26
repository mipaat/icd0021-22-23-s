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
    public class QueueItemController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public QueueItemController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: QueueItem
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.QueueItems.Include(q => q.AddedBy).Include(q => q.ApprovedBy).Include(q => q.Author).Include(q => q.Playlist).Include(q => q.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: QueueItem/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.QueueItems == null)
            {
                return NotFound();
            }

            var queueItem = await _context.QueueItems
                .Include(q => q.AddedBy)
                .Include(q => q.ApprovedBy)
                .Include(q => q.Author)
                .Include(q => q.Playlist)
                .Include(q => q.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (queueItem == null)
            {
                return NotFound();
            }

            return View(queueItem);
        }

        // GET: QueueItem/Create
        public IActionResult Create()
        {
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ApprovedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: QueueItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Url,Platform,IdOnPlatform,Monitor,Download,WebHookUrl,WebhookSecret,WebhookData,AddedById,AddedAt,ApprovedById,ApprovedAt,CompletedAt,AuthorId,VideoId,PlaylistId,Id")] QueueItem queueItem)
        {
            if (ModelState.IsValid)
            {
                queueItem.Id = Guid.NewGuid();
                _context.Add(queueItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id", queueItem.AddedById);
            ViewData["ApprovedById"] = new SelectList(_context.Users, "Id", "Id", queueItem.ApprovedById);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", queueItem.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", queueItem.PlaylistId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", queueItem.VideoId);
            return View(queueItem);
        }

        // GET: QueueItem/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.QueueItems == null)
            {
                return NotFound();
            }

            var queueItem = await _context.QueueItems.FindAsync(id);
            if (queueItem == null)
            {
                return NotFound();
            }
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id", queueItem.AddedById);
            ViewData["ApprovedById"] = new SelectList(_context.Users, "Id", "Id", queueItem.ApprovedById);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", queueItem.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", queueItem.PlaylistId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", queueItem.VideoId);
            return View(queueItem);
        }

        // POST: QueueItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Url,Platform,IdOnPlatform,Monitor,Download,WebHookUrl,WebhookSecret,WebhookData,AddedById,AddedAt,ApprovedById,ApprovedAt,CompletedAt,AuthorId,VideoId,PlaylistId,Id")] QueueItem queueItem)
        {
            if (id != queueItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(queueItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QueueItemExists(queueItem.Id))
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
            ViewData["AddedById"] = new SelectList(_context.Users, "Id", "Id", queueItem.AddedById);
            ViewData["ApprovedById"] = new SelectList(_context.Users, "Id", "Id", queueItem.ApprovedById);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", queueItem.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", queueItem.PlaylistId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", queueItem.VideoId);
            return View(queueItem);
        }

        // GET: QueueItem/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.QueueItems == null)
            {
                return NotFound();
            }

            var queueItem = await _context.QueueItems
                .Include(q => q.AddedBy)
                .Include(q => q.ApprovedBy)
                .Include(q => q.Author)
                .Include(q => q.Playlist)
                .Include(q => q.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (queueItem == null)
            {
                return NotFound();
            }

            return View(queueItem);
        }

        // POST: QueueItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.QueueItems == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.QueueItems'  is null.");
            }
            var queueItem = await _context.QueueItems.FindAsync(id);
            if (queueItem != null)
            {
                _context.QueueItems.Remove(queueItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QueueItemExists(Guid id)
        {
          return (_context.QueueItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
