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
    public class VideoCategoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public VideoCategoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: VideoCategory
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.VideoCategories.Include(v => v.Category).Include(v => v.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: VideoCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.VideoCategories == null)
            {
                return NotFound();
            }

            var videoCategory = await _context.VideoCategories
                .Include(v => v.Category)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoCategory == null)
            {
                return NotFound();
            }

            return View(videoCategory);
        }

        // GET: VideoCategory/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: VideoCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,CategoryId,AutoAssign,Id")] VideoCategory videoCategory)
        {
            if (ModelState.IsValid)
            {
                videoCategory.Id = Guid.NewGuid();
                _context.Add(videoCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", videoCategory.CategoryId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoCategory.VideoId);
            return View(videoCategory);
        }

        // GET: VideoCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.VideoCategories == null)
            {
                return NotFound();
            }

            var videoCategory = await _context.VideoCategories.FindAsync(id);
            if (videoCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", videoCategory.CategoryId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoCategory.VideoId);
            return View(videoCategory);
        }

        // POST: VideoCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,CategoryId,AutoAssign,Id")] VideoCategory videoCategory)
        {
            if (id != videoCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoCategoryExists(videoCategory.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", videoCategory.CategoryId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoCategory.VideoId);
            return View(videoCategory);
        }

        // GET: VideoCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.VideoCategories == null)
            {
                return NotFound();
            }

            var videoCategory = await _context.VideoCategories
                .Include(v => v.Category)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoCategory == null)
            {
                return NotFound();
            }

            return View(videoCategory);
        }

        // POST: VideoCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.VideoCategories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.VideoCategories'  is null.");
            }
            var videoCategory = await _context.VideoCategories.FindAsync(id);
            if (videoCategory != null)
            {
                _context.VideoCategories.Remove(videoCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoCategoryExists(Guid id)
        {
          return (_context.VideoCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
