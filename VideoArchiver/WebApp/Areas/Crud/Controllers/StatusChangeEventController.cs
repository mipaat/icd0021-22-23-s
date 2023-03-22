using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class StatusChangeEventController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public StatusChangeEventController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: StatusChangeEvent
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.StatusChangeEvents.Include(s => s.Author).Include(s => s.Playlist).Include(s => s.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: StatusChangeEvent/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.StatusChangeEvents == null)
            {
                return NotFound();
            }

            var statusChangeEvent = await _context.StatusChangeEvents
                .Include(s => s.Author)
                .Include(s => s.Playlist)
                .Include(s => s.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusChangeEvent == null)
            {
                return NotFound();
            }

            return View(statusChangeEvent);
        }

        // GET: StatusChangeEvent/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: StatusChangeEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PreviousAvailability,NewAvailability,PreviousPrivacyStatus,NewPrivacyStatus,OccurredAt,AuthorId,VideoId,PlaylistId,Id")] StatusChangeEvent statusChangeEvent)
        {
            if (ModelState.IsValid)
            {
                statusChangeEvent.Id = Guid.NewGuid();
                _context.Add(statusChangeEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", statusChangeEvent.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", statusChangeEvent.PlaylistId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", statusChangeEvent.VideoId);
            return View(statusChangeEvent);
        }

        // GET: StatusChangeEvent/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.StatusChangeEvents == null)
            {
                return NotFound();
            }

            var statusChangeEvent = await _context.StatusChangeEvents.FindAsync(id);
            if (statusChangeEvent == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", statusChangeEvent.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", statusChangeEvent.PlaylistId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", statusChangeEvent.VideoId);
            return View(statusChangeEvent);
        }

        // POST: StatusChangeEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PreviousAvailability,NewAvailability,PreviousPrivacyStatus,NewPrivacyStatus,OccurredAt,AuthorId,VideoId,PlaylistId,Id")] StatusChangeEvent statusChangeEvent)
        {
            if (id != statusChangeEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusChangeEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusChangeEventExists(statusChangeEvent.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", statusChangeEvent.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", statusChangeEvent.PlaylistId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", statusChangeEvent.VideoId);
            return View(statusChangeEvent);
        }

        // GET: StatusChangeEvent/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.StatusChangeEvents == null)
            {
                return NotFound();
            }

            var statusChangeEvent = await _context.StatusChangeEvents
                .Include(s => s.Author)
                .Include(s => s.Playlist)
                .Include(s => s.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusChangeEvent == null)
            {
                return NotFound();
            }

            return View(statusChangeEvent);
        }

        // POST: StatusChangeEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.StatusChangeEvents == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.StatusChangeEvents'  is null.");
            }
            var statusChangeEvent = await _context.StatusChangeEvents.FindAsync(id);
            if (statusChangeEvent != null)
            {
                _context.StatusChangeEvents.Remove(statusChangeEvent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusChangeEventExists(Guid id)
        {
          return (_context.StatusChangeEvents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
