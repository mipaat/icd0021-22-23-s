using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class VideoRatingController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public VideoRatingController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: VideoRating
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.VideoRatings.Include(v => v.Author).Include(v => v.Category).Include(v => v.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: VideoRating/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.VideoRatings == null)
            {
                return NotFound();
            }

            var videoRating = await _context.VideoRatings
                .Include(v => v.Author)
                .Include(v => v.Category)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoRating == null)
            {
                return NotFound();
            }

            return View(videoRating);
        }

        // GET: VideoRating/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: VideoRating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,AuthorId,Rating,Comment,CategoryId,Id")] VideoRating videoRating)
        {
            if (ModelState.IsValid)
            {
                videoRating.Id = Guid.NewGuid();
                _context.Add(videoRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoRating.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", videoRating.CategoryId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoRating.VideoId);
            return View(videoRating);
        }

        // GET: VideoRating/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.VideoRatings == null)
            {
                return NotFound();
            }

            var videoRating = await _context.VideoRatings.FindAsync(id);
            if (videoRating == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoRating.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", videoRating.CategoryId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoRating.VideoId);
            return View(videoRating);
        }

        // POST: VideoRating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,AuthorId,Rating,Comment,CategoryId,Id")] VideoRating videoRating)
        {
            if (id != videoRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoRatingExists(videoRating.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoRating.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", videoRating.CategoryId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoRating.VideoId);
            return View(videoRating);
        }

        // GET: VideoRating/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.VideoRatings == null)
            {
                return NotFound();
            }

            var videoRating = await _context.VideoRatings
                .Include(v => v.Author)
                .Include(v => v.Category)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoRating == null)
            {
                return NotFound();
            }

            return View(videoRating);
        }

        // POST: VideoRating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.VideoRatings == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.VideoRatings'  is null.");
            }
            var videoRating = await _context.VideoRatings.FindAsync(id);
            if (videoRating != null)
            {
                _context.VideoRatings.Remove(videoRating);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoRatingExists(Guid id)
        {
          return (_context.VideoRatings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
