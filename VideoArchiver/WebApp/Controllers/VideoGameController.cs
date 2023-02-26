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
    public class VideoGameController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public VideoGameController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: VideoGame
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.VideoGames.Include(v => v.Game).Include(v => v.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: VideoGame/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.VideoGames == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGames
                .Include(v => v.Game)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoGame == null)
            {
                return NotFound();
            }

            return View(videoGame);
        }

        // GET: VideoGame/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "IgdbId");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: VideoGame/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,GameId,IgdbId,Platform,IdOnPlatform,Name,BoxArtUrl,FromTimecode,ToTimecode,ValidSince,ValidUntil,Id")] VideoGame videoGame)
        {
            if (ModelState.IsValid)
            {
                videoGame.Id = Guid.NewGuid();
                _context.Add(videoGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "IgdbId", videoGame.GameId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoGame.VideoId);
            return View(videoGame);
        }

        // GET: VideoGame/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.VideoGames == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGames.FindAsync(id);
            if (videoGame == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "IgdbId", videoGame.GameId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoGame.VideoId);
            return View(videoGame);
        }

        // POST: VideoGame/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,GameId,IgdbId,Platform,IdOnPlatform,Name,BoxArtUrl,FromTimecode,ToTimecode,ValidSince,ValidUntil,Id")] VideoGame videoGame)
        {
            if (id != videoGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGameExists(videoGame.Id))
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
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "IgdbId", videoGame.GameId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoGame.VideoId);
            return View(videoGame);
        }

        // GET: VideoGame/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.VideoGames == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGames
                .Include(v => v.Game)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoGame == null)
            {
                return NotFound();
            }

            return View(videoGame);
        }

        // POST: VideoGame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.VideoGames == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.VideoGames'  is null.");
            }
            var videoGame = await _context.VideoGames.FindAsync(id);
            if (videoGame != null)
            {
                _context.VideoGames.Remove(videoGame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoGameExists(Guid id)
        {
          return (_context.VideoGames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
