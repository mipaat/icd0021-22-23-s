using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistSubscriptionController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistSubscriptionController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistSubscription
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.PlaylistSubscriptions.Include(p => p.Playlist).Include(p => p.Subscriber);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: PlaylistSubscription/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PlaylistSubscriptions == null)
            {
                return NotFound();
            }

            var playlistSubscription = await _context.PlaylistSubscriptions
                .Include(p => p.Playlist)
                .Include(p => p.Subscriber)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistSubscription == null)
            {
                return NotFound();
            }

            return View(playlistSubscription);
        }

        // GET: PlaylistSubscription/Create
        public IActionResult Create()
        {
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform");
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            return View();
        }

        // POST: PlaylistSubscription/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,SubscriberId,Priority,Id")] PlaylistSubscription playlistSubscription)
        {
            if (ModelState.IsValid)
            {
                playlistSubscription.Id = Guid.NewGuid();
                _context.Add(playlistSubscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistSubscription.PlaylistId);
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistSubscription.SubscriberId);
            return View(playlistSubscription);
        }

        // GET: PlaylistSubscription/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PlaylistSubscriptions == null)
            {
                return NotFound();
            }

            var playlistSubscription = await _context.PlaylistSubscriptions.FindAsync(id);
            if (playlistSubscription == null)
            {
                return NotFound();
            }
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistSubscription.PlaylistId);
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistSubscription.SubscriberId);
            return View(playlistSubscription);
        }

        // POST: PlaylistSubscription/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,SubscriberId,Priority,Id")] PlaylistSubscription playlistSubscription)
        {
            if (id != playlistSubscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistSubscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistSubscriptionExists(playlistSubscription.Id))
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
            ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "IdOnPlatform", playlistSubscription.PlaylistId);
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", playlistSubscription.SubscriberId);
            return View(playlistSubscription);
        }

        // GET: PlaylistSubscription/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PlaylistSubscriptions == null)
            {
                return NotFound();
            }

            var playlistSubscription = await _context.PlaylistSubscriptions
                .Include(p => p.Playlist)
                .Include(p => p.Subscriber)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistSubscription == null)
            {
                return NotFound();
            }

            return View(playlistSubscription);
        }

        // POST: PlaylistSubscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PlaylistSubscriptions == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.PlaylistSubscriptions'  is null.");
            }
            var playlistSubscription = await _context.PlaylistSubscriptions.FindAsync(id);
            if (playlistSubscription != null)
            {
                _context.PlaylistSubscriptions.Remove(playlistSubscription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistSubscriptionExists(Guid id)
        {
          return (_context.PlaylistSubscriptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
