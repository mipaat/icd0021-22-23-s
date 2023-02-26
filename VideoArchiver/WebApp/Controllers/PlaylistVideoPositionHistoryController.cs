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
    public class PlaylistVideoPositionHistoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public PlaylistVideoPositionHistoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistVideoPositionHistory
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.PlaylistVideoPositionHistories.Include(p => p.PlaylistVideo);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: PlaylistVideoPositionHistory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.PlaylistVideoPositionHistories == null)
            {
                return NotFound();
            }

            var playlistVideoPositionHistory = await _context.PlaylistVideoPositionHistories
                .Include(p => p.PlaylistVideo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistVideoPositionHistory == null)
            {
                return NotFound();
            }

            return View(playlistVideoPositionHistory);
        }

        // GET: PlaylistVideoPositionHistory/Create
        public IActionResult Create()
        {
            ViewData["PlaylistVideoId"] = new SelectList(_context.PlaylistVideos, "Id", "Id");
            return View();
        }

        // POST: PlaylistVideoPositionHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistVideoId,Position,ValidSince,ValidUntil,Id")] PlaylistVideoPositionHistory playlistVideoPositionHistory)
        {
            if (ModelState.IsValid)
            {
                playlistVideoPositionHistory.Id = Guid.NewGuid();
                _context.Add(playlistVideoPositionHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaylistVideoId"] = new SelectList(_context.PlaylistVideos, "Id", "Id", playlistVideoPositionHistory.PlaylistVideoId);
            return View(playlistVideoPositionHistory);
        }

        // GET: PlaylistVideoPositionHistory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.PlaylistVideoPositionHistories == null)
            {
                return NotFound();
            }

            var playlistVideoPositionHistory = await _context.PlaylistVideoPositionHistories.FindAsync(id);
            if (playlistVideoPositionHistory == null)
            {
                return NotFound();
            }
            ViewData["PlaylistVideoId"] = new SelectList(_context.PlaylistVideos, "Id", "Id", playlistVideoPositionHistory.PlaylistVideoId);
            return View(playlistVideoPositionHistory);
        }

        // POST: PlaylistVideoPositionHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistVideoId,Position,ValidSince,ValidUntil,Id")] PlaylistVideoPositionHistory playlistVideoPositionHistory)
        {
            if (id != playlistVideoPositionHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistVideoPositionHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistVideoPositionHistoryExists(playlistVideoPositionHistory.Id))
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
            ViewData["PlaylistVideoId"] = new SelectList(_context.PlaylistVideos, "Id", "Id", playlistVideoPositionHistory.PlaylistVideoId);
            return View(playlistVideoPositionHistory);
        }

        // GET: PlaylistVideoPositionHistory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.PlaylistVideoPositionHistories == null)
            {
                return NotFound();
            }

            var playlistVideoPositionHistory = await _context.PlaylistVideoPositionHistories
                .Include(p => p.PlaylistVideo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playlistVideoPositionHistory == null)
            {
                return NotFound();
            }

            return View(playlistVideoPositionHistory);
        }

        // POST: PlaylistVideoPositionHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.PlaylistVideoPositionHistories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.PlaylistVideoPositionHistories'  is null.");
            }
            var playlistVideoPositionHistory = await _context.PlaylistVideoPositionHistories.FindAsync(id);
            if (playlistVideoPositionHistory != null)
            {
                _context.PlaylistVideoPositionHistories.Remove(playlistVideoPositionHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistVideoPositionHistoryExists(Guid id)
        {
          return (_context.PlaylistVideoPositionHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
