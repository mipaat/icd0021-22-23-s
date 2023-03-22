using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistVideoController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistVideoController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistVideo
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.PlaylistVideos.Include(p => p.AddedBy).Include(p => p.Playlist).Include(p => p.RemovedBy).Include(p => p.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: PlaylistVideo/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PlaylistVideos == null)
            {
                return NotFound();
            }

            var playlistVideo = await _context.PlaylistVideos
                .Include(p => p.AddedBy)
                .Include(p => p.Playlist)
                .Include(p => p.RemovedBy)
                .Include(p => p.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistVideo == null)
            {
                return NotFound();
            }

            return View(playlistVideo);
        }

        // GET: PlaylistVideo/Create
        public IActionResult Create()
        {
            ViewData["AddedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            ViewData["RemovedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: PlaylistVideo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,VideoId,Position,AddedAt,RemovedAt,AddedById,RemovedById,Id")] PlaylistVideo playlistVideo)
        {
            if (ModelState.IsValid)
            {
                playlistVideo.Id = Guid.NewGuid();
                _context.Add(playlistVideo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistVideo.AddedById);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistVideo.PlaylistId);
            ViewData["RemovedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistVideo.RemovedById);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", playlistVideo.VideoId);
            return View(playlistVideo);
        }

        // GET: PlaylistVideo/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PlaylistVideos == null)
            {
                return NotFound();
            }

            var playlistVideo = await _context.PlaylistVideos.FindAsync(id);
            if (playlistVideo == null)
            {
                return NotFound();
            }
            ViewData["AddedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistVideo.AddedById);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistVideo.PlaylistId);
            ViewData["RemovedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistVideo.RemovedById);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", playlistVideo.VideoId);
            return View(playlistVideo);
        }

        // POST: PlaylistVideo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,VideoId,Position,AddedAt,RemovedAt,AddedById,RemovedById,Id")] PlaylistVideo playlistVideo)
        {
            if (id != playlistVideo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistVideo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistVideoExists(playlistVideo.Id))
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
            ViewData["AddedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistVideo.AddedById);
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistVideo.PlaylistId);
            ViewData["RemovedById"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistVideo.RemovedById);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", playlistVideo.VideoId);
            return View(playlistVideo);
        }

        // GET: PlaylistVideo/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PlaylistVideos == null)
            {
                return NotFound();
            }

            var playlistVideo = await _context.PlaylistVideos
                .Include(p => p.AddedBy)
                .Include(p => p.Playlist)
                .Include(p => p.RemovedBy)
                .Include(p => p.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistVideo == null)
            {
                return NotFound();
            }

            return View(playlistVideo);
        }

        // POST: PlaylistVideo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PlaylistVideos == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.PlaylistVideos'  is null.");
            }
            var playlistVideo = await _context.PlaylistVideos.FindAsync(id);
            if (playlistVideo != null)
            {
                _context.PlaylistVideos.Remove(playlistVideo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistVideoExists(Guid id)
        {
          return (_context.PlaylistVideos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
