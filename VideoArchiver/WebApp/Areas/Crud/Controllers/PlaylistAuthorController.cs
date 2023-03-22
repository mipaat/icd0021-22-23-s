using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistAuthorController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistAuthorController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistAuthor
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.PlaylistAuthors.Include(p => p.Author).Include(p => p.Playlist);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: PlaylistAuthor/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PlaylistAuthors == null)
            {
                return NotFound();
            }

            var playlistAuthor = await _context.PlaylistAuthors
                .Include(p => p.Author)
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistAuthor == null)
            {
                return NotFound();
            }

            return View(playlistAuthor);
        }

        // GET: PlaylistAuthor/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            return View();
        }

        // POST: PlaylistAuthor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,AuthorId,Role,Id")] PlaylistAuthor playlistAuthor)
        {
            if (ModelState.IsValid)
            {
                playlistAuthor.Id = Guid.NewGuid();
                _context.Add(playlistAuthor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistAuthor.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistAuthor.PlaylistId);
            return View(playlistAuthor);
        }

        // GET: PlaylistAuthor/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PlaylistAuthors == null)
            {
                return NotFound();
            }

            var playlistAuthor = await _context.PlaylistAuthors.FindAsync(id);
            if (playlistAuthor == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistAuthor.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistAuthor.PlaylistId);
            return View(playlistAuthor);
        }

        // POST: PlaylistAuthor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,AuthorId,Role,Id")] PlaylistAuthor playlistAuthor)
        {
            if (id != playlistAuthor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistAuthor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistAuthorExists(playlistAuthor.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistAuthor.AuthorId);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistAuthor.PlaylistId);
            return View(playlistAuthor);
        }

        // GET: PlaylistAuthor/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PlaylistAuthors == null)
            {
                return NotFound();
            }

            var playlistAuthor = await _context.PlaylistAuthors
                .Include(p => p.Author)
                .Include(p => p.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistAuthor == null)
            {
                return NotFound();
            }

            return View(playlistAuthor);
        }

        // POST: PlaylistAuthor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PlaylistAuthors == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.PlaylistAuthors'  is null.");
            }
            var playlistAuthor = await _context.PlaylistAuthors.FindAsync(id);
            if (playlistAuthor != null)
            {
                _context.PlaylistAuthors.Remove(playlistAuthor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistAuthorExists(Guid id)
        {
          return (_context.PlaylistAuthors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
