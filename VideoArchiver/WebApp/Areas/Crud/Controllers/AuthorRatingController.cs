using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorRatingController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public AuthorRatingController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: AuthorRating
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.AuthorRatings.Include(a => a.Category).Include(a => a.Rated).Include(a => a.Rater);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: AuthorRating/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.AuthorRatings == null)
            {
                return NotFound();
            }

            var authorRating = await _context.AuthorRatings
                .Include(a => a.Category)
                .Include(a => a.Rated)
                .Include(a => a.Rater)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorRating == null)
            {
                return NotFound();
            }

            return View(authorRating);
        }

        // GET: AuthorRating/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["RatedId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["RaterId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            return View();
        }

        // POST: AuthorRating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatedId,RaterId,Rating,Comment,CategoryId,Id")] AuthorRating authorRating)
        {
            if (ModelState.IsValid)
            {
                authorRating.Id = Guid.NewGuid();
                _context.Add(authorRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", authorRating.CategoryId);
            ViewData["RatedId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorRating.RatedId);
            ViewData["RaterId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorRating.RaterId);
            return View(authorRating);
        }

        // GET: AuthorRating/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.AuthorRatings == null)
            {
                return NotFound();
            }

            var authorRating = await _context.AuthorRatings.FindAsync(id);
            if (authorRating == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", authorRating.CategoryId);
            ViewData["RatedId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorRating.RatedId);
            ViewData["RaterId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorRating.RaterId);
            return View(authorRating);
        }

        // POST: AuthorRating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RatedId,RaterId,Rating,Comment,CategoryId,Id")] AuthorRating authorRating)
        {
            if (id != authorRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorRatingExists(authorRating.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", authorRating.CategoryId);
            ViewData["RatedId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorRating.RatedId);
            ViewData["RaterId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorRating.RaterId);
            return View(authorRating);
        }

        // GET: AuthorRating/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.AuthorRatings == null)
            {
                return NotFound();
            }

            var authorRating = await _context.AuthorRatings
                .Include(a => a.Category)
                .Include(a => a.Rated)
                .Include(a => a.Rater)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorRating == null)
            {
                return NotFound();
            }

            return View(authorRating);
        }

        // POST: AuthorRating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.AuthorRatings == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.AuthorRatings'  is null.");
            }
            var authorRating = await _context.AuthorRatings.FindAsync(id);
            if (authorRating != null)
            {
                _context.AuthorRatings.Remove(authorRating);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorRatingExists(Guid id)
        {
          return (_context.AuthorRatings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
