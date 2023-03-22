using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistRatingController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistRatingController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistRating
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.PlaylistRatings.Include(p => p.Author).Include(p => p.Category).Include(p => p.Playlist);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: PlaylistRating/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PlaylistRatings == null)
            {
                return NotFound();
            }

            var playlistRating = await _context.PlaylistRatings
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistRating == null)
            {
                return NotFound();
            }

            return View(playlistRating);
        }

        // GET: PlaylistRating/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            return View();
        }

        // POST: PlaylistRating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,AuthorId,Rating,Comment,CategoryId,Id")] PlaylistRating playlistRating)
        {
            if (ModelState.IsValid)
            {
                playlistRating.Id = Guid.NewGuid();
                _context.Add(playlistRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistRating.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", playlistRating.CategoryId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistRating.PlaylistId);
            return View(playlistRating);
        }

        // GET: PlaylistRating/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PlaylistRatings == null)
            {
                return NotFound();
            }

            var playlistRating = await _context.PlaylistRatings.FindAsync(id);
            if (playlistRating == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistRating.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", playlistRating.CategoryId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistRating.PlaylistId);
            return View(playlistRating);
        }

        // POST: PlaylistRating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,AuthorId,Rating,Comment,CategoryId,Id")] PlaylistRating playlistRating)
        {
            if (id != playlistRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistRatingExists(playlistRating.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistRating.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", playlistRating.CategoryId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistRating.PlaylistId);
            return View(playlistRating);
        }

        // GET: PlaylistRating/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PlaylistRatings == null)
            {
                return NotFound();
            }

            var playlistRating = await _context.PlaylistRatings
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistRating == null)
            {
                return NotFound();
            }

            return View(playlistRating);
        }

        // POST: PlaylistRating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PlaylistRatings == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.PlaylistRatings'  is null.");
            }
            var playlistRating = await _context.PlaylistRatings.FindAsync(id);
            if (playlistRating != null)
            {
                _context.PlaylistRatings.Remove(playlistRating);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistRatingExists(Guid id)
        {
          return (_context.PlaylistRatings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
