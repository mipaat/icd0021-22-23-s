using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistCategoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistCategoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistCategory
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.PlaylistCategories.Include(p => p.Category).Include(p => p.Playlist);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: PlaylistCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PlaylistCategories == null)
            {
                return NotFound();
            }

            var playlistCategory = await _context.PlaylistCategories
                .Include(p => p.Category)
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistCategory == null)
            {
                return NotFound();
            }

            return View(playlistCategory);
        }

        // GET: PlaylistCategory/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            return View();
        }

        // POST: PlaylistCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,CategoryId,AutoAssign,Id")] PlaylistCategory playlistCategory)
        {
            if (ModelState.IsValid)
            {
                playlistCategory.Id = Guid.NewGuid();
                _context.Add(playlistCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", playlistCategory.CategoryId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistCategory.PlaylistId);
            return View(playlistCategory);
        }

        // GET: PlaylistCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PlaylistCategories == null)
            {
                return NotFound();
            }

            var playlistCategory = await _context.PlaylistCategories.FindAsync(id);
            if (playlistCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", playlistCategory.CategoryId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistCategory.PlaylistId);
            return View(playlistCategory);
        }

        // POST: PlaylistCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,CategoryId,AutoAssign,Id")] PlaylistCategory playlistCategory)
        {
            if (id != playlistCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistCategoryExists(playlistCategory.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", playlistCategory.CategoryId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistCategory.PlaylistId);
            return View(playlistCategory);
        }

        // GET: PlaylistCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PlaylistCategories == null)
            {
                return NotFound();
            }

            var playlistCategory = await _context.PlaylistCategories
                .Include(p => p.Category)
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistCategory == null)
            {
                return NotFound();
            }

            return View(playlistCategory);
        }

        // POST: PlaylistCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PlaylistCategories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.PlaylistCategories'  is null.");
            }
            var playlistCategory = await _context.PlaylistCategories.FindAsync(id);
            if (playlistCategory != null)
            {
                _context.PlaylistCategories.Remove(playlistCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistCategoryExists(Guid id)
        {
          return (_context.PlaylistCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
