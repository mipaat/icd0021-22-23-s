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
    public class PlaylistController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: Playlist
        public async Task<IActionResult> Index()
        {
              return _context.Playlists != null ? 
                          View(await _context.Playlists.ToListAsync()) :
                          Problem("Entity set 'AbstractAppDbContext.Playlists'  is null.");
        }

        // GET: Playlist/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Playlists == null)
            {
                return NotFound();
            }

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // GET: Playlist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Playlist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Platform,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                playlist.Id = Guid.NewGuid();
                _context.Add(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(playlist);
        }

        // GET: Playlist/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Playlists == null)
            {
                return NotFound();
            }

            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // POST: Playlist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Platform,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")] Playlist playlist)
        {
            if (id != playlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistExists(playlist.Id))
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
            return View(playlist);
        }

        // GET: Playlist/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Playlists == null)
            {
                return NotFound();
            }

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // POST: Playlist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Playlists == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.Playlists'  is null.");
            }
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistExists(Guid id)
        {
          return (_context.Playlists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
