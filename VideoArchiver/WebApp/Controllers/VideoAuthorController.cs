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
    public class VideoAuthorController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public VideoAuthorController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: VideoAuthor
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.VideoAuthors.Include(v => v.Author).Include(v => v.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: VideoAuthor/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.VideoAuthors == null)
            {
                return NotFound();
            }

            var videoAuthor = await _context.VideoAuthors
                .Include(v => v.Author)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoAuthor == null)
            {
                return NotFound();
            }

            return View(videoAuthor);
        }

        // GET: VideoAuthor/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: VideoAuthor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,AuthorId,Role,Id")] VideoAuthor videoAuthor)
        {
            if (ModelState.IsValid)
            {
                videoAuthor.Id = Guid.NewGuid();
                _context.Add(videoAuthor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoAuthor.AuthorId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoAuthor.VideoId);
            return View(videoAuthor);
        }

        // GET: VideoAuthor/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.VideoAuthors == null)
            {
                return NotFound();
            }

            var videoAuthor = await _context.VideoAuthors.FindAsync(id);
            if (videoAuthor == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoAuthor.AuthorId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoAuthor.VideoId);
            return View(videoAuthor);
        }

        // POST: VideoAuthor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,AuthorId,Role,Id")] VideoAuthor videoAuthor)
        {
            if (id != videoAuthor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoAuthor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoAuthorExists(videoAuthor.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoAuthor.AuthorId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoAuthor.VideoId);
            return View(videoAuthor);
        }

        // GET: VideoAuthor/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.VideoAuthors == null)
            {
                return NotFound();
            }

            var videoAuthor = await _context.VideoAuthors
                .Include(v => v.Author)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoAuthor == null)
            {
                return NotFound();
            }

            return View(videoAuthor);
        }

        // POST: VideoAuthor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.VideoAuthors == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.VideoAuthors'  is null.");
            }
            var videoAuthor = await _context.VideoAuthors.FindAsync(id);
            if (videoAuthor != null)
            {
                _context.VideoAuthors.Remove(videoAuthor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoAuthorExists(Guid id)
        {
          return (_context.VideoAuthors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
