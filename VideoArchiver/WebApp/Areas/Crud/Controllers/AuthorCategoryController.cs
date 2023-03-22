using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorCategoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public AuthorCategoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: AuthorCategory
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.AuthorCategories.Include(a => a.Author).Include(a => a.Category);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: AuthorCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.AuthorCategories == null)
            {
                return NotFound();
            }

            var authorCategory = await _context.AuthorCategories
                .Include(a => a.Author)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorCategory == null)
            {
                return NotFound();
            }

            return View(authorCategory);
        }

        // GET: AuthorCategory/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: AuthorCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,CategoryId,AutoAssign,Id")] AuthorCategory authorCategory)
        {
            if (ModelState.IsValid)
            {
                authorCategory.Id = Guid.NewGuid();
                _context.Add(authorCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorCategory.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", authorCategory.CategoryId);
            return View(authorCategory);
        }

        // GET: AuthorCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.AuthorCategories == null)
            {
                return NotFound();
            }

            var authorCategory = await _context.AuthorCategories.FindAsync(id);
            if (authorCategory == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorCategory.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", authorCategory.CategoryId);
            return View(authorCategory);
        }

        // POST: AuthorCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AuthorId,CategoryId,AutoAssign,Id")] AuthorCategory authorCategory)
        {
            if (id != authorCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorCategoryExists(authorCategory.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorCategory.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", authorCategory.CategoryId);
            return View(authorCategory);
        }

        // GET: AuthorCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.AuthorCategories == null)
            {
                return NotFound();
            }

            var authorCategory = await _context.AuthorCategories
                .Include(a => a.Author)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorCategory == null)
            {
                return NotFound();
            }

            return View(authorCategory);
        }

        // POST: AuthorCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.AuthorCategories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.AuthorCategories'  is null.");
            }
            var authorCategory = await _context.AuthorCategories.FindAsync(id);
            if (authorCategory != null)
            {
                _context.AuthorCategories.Remove(authorCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorCategoryExists(Guid id)
        {
          return (_context.AuthorCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
