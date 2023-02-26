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
    public class AuthorHistoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public AuthorHistoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: AuthorHistory
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.AuthorHistories.Include(a => a.Author);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: AuthorHistory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.AuthorHistories == null)
            {
                return NotFound();
            }

            var authorHistory = await _context.AuthorHistories
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorHistory == null)
            {
                return NotFound();
            }

            return View(authorHistory);
        }

        // GET: AuthorHistory/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            return View();
        }

        // POST: AuthorHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,LastValidAt,InternalPrivacyStatus,Id")] AuthorHistory authorHistory)
        {
            if (ModelState.IsValid)
            {
                authorHistory.Id = Guid.NewGuid();
                _context.Add(authorHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorHistory.AuthorId);
            return View(authorHistory);
        }

        // GET: AuthorHistory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.AuthorHistories == null)
            {
                return NotFound();
            }

            var authorHistory = await _context.AuthorHistories.FindAsync(id);
            if (authorHistory == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorHistory.AuthorId);
            return View(authorHistory);
        }

        // POST: AuthorHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AuthorId,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,LastValidAt,InternalPrivacyStatus,Id")] AuthorHistory authorHistory)
        {
            if (id != authorHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorHistoryExists(authorHistory.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorHistory.AuthorId);
            return View(authorHistory);
        }

        // GET: AuthorHistory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.AuthorHistories == null)
            {
                return NotFound();
            }

            var authorHistory = await _context.AuthorHistories
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorHistory == null)
            {
                return NotFound();
            }

            return View(authorHistory);
        }

        // POST: AuthorHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.AuthorHistories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.AuthorHistories'  is null.");
            }
            var authorHistory = await _context.AuthorHistories.FindAsync(id);
            if (authorHistory != null)
            {
                _context.AuthorHistories.Remove(authorHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorHistoryExists(Guid id)
        {
          return (_context.AuthorHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
