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
    public class AuthorPubSubController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public AuthorPubSubController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: AuthorPubSub
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.AuthorPubSubs.Include(a => a.Author);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: AuthorPubSub/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.AuthorPubSubs == null)
            {
                return NotFound();
            }

            var authorPubSub = await _context.AuthorPubSubs
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorPubSub == null)
            {
                return NotFound();
            }

            return View(authorPubSub);
        }

        // GET: AuthorPubSub/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            return View();
        }

        // POST: AuthorPubSub/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeasedAt,LeaseDuration,Secret,AuthorId,Id")] AuthorPubSub authorPubSub)
        {
            if (ModelState.IsValid)
            {
                authorPubSub.Id = Guid.NewGuid();
                _context.Add(authorPubSub);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorPubSub.AuthorId);
            return View(authorPubSub);
        }

        // GET: AuthorPubSub/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.AuthorPubSubs == null)
            {
                return NotFound();
            }

            var authorPubSub = await _context.AuthorPubSubs.FindAsync(id);
            if (authorPubSub == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorPubSub.AuthorId);
            return View(authorPubSub);
        }

        // POST: AuthorPubSub/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LeasedAt,LeaseDuration,Secret,AuthorId,Id")] AuthorPubSub authorPubSub)
        {
            if (id != authorPubSub.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorPubSub);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorPubSubExists(authorPubSub.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorPubSub.AuthorId);
            return View(authorPubSub);
        }

        // GET: AuthorPubSub/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.AuthorPubSubs == null)
            {
                return NotFound();
            }

            var authorPubSub = await _context.AuthorPubSubs
                .Include(a => a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorPubSub == null)
            {
                return NotFound();
            }

            return View(authorPubSub);
        }

        // POST: AuthorPubSub/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.AuthorPubSubs == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.AuthorPubSubs'  is null.");
            }
            var authorPubSub = await _context.AuthorPubSubs.FindAsync(id);
            if (authorPubSub != null)
            {
                _context.AuthorPubSubs.Remove(authorPubSub);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorPubSubExists(Guid id)
        {
          return (_context.AuthorPubSubs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
