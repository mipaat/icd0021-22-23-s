using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class CategoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public CategoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.Categories.Include(c => c.Creator).Include(c => c.ParentCategory);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Creator)
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsPublic,SupportsAuthors,SupportsVideos,SupportsPlaylists,IsAssignable,ParentCategoryId,Platform,CreatorId,Id")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", category.CreatorId);
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentCategoryId);
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["CreatorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", category.CreatorId);
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentCategoryId);
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,IsPublic,SupportsAuthors,SupportsVideos,SupportsPlaylists,IsAssignable,ParentCategoryId,Platform,CreatorId,Id")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["CreatorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", category.CreatorId);
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Id", category.ParentCategoryId);
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Creator)
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
