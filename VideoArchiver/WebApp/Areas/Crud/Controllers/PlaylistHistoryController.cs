using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistHistoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistHistoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistHistory
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.PlaylistHistories.Include(p => p.Playlist);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: PlaylistHistory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PlaylistHistories == null)
            {
                return NotFound();
            }

            var playlistHistory = await _context.PlaylistHistories
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistHistory == null)
            {
                return NotFound();
            }

            return View(playlistHistory);
        }

        // GET: PlaylistHistory/Create
        public IActionResult Create()
        {
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            return View();
        }

        // POST: PlaylistHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,LastValidAt,InternalPrivacyStatus,Id")] PlaylistHistory playlistHistory)
        {
            if (ModelState.IsValid)
            {
                playlistHistory.Id = Guid.NewGuid();
                _context.Add(playlistHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistHistory.PlaylistId);
            return View(playlistHistory);
        }

        // GET: PlaylistHistory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PlaylistHistories == null)
            {
                return NotFound();
            }

            var playlistHistory = await _context.PlaylistHistories.FindAsync(id);
            if (playlistHistory == null)
            {
                return NotFound();
            }
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistHistory.PlaylistId);
            return View(playlistHistory);
        }

        // POST: PlaylistHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,LastValidAt,InternalPrivacyStatus,Id")] PlaylistHistory playlistHistory)
        {
            if (id != playlistHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistHistoryExists(playlistHistory.Id))
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
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistHistory.PlaylistId);
            return View(playlistHistory);
        }

        // GET: PlaylistHistory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PlaylistHistories == null)
            {
                return NotFound();
            }

            var playlistHistory = await _context.PlaylistHistories
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistHistory == null)
            {
                return NotFound();
            }

            return View(playlistHistory);
        }

        // POST: PlaylistHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PlaylistHistories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.PlaylistHistories'  is null.");
            }
            var playlistHistory = await _context.PlaylistHistories.FindAsync(id);
            if (playlistHistory != null)
            {
                _context.PlaylistHistories.Remove(playlistHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistHistoryExists(Guid id)
        {
          return (_context.PlaylistHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
