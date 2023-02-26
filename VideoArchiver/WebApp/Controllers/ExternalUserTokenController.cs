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
    public class ExternalUserTokenController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public ExternalUserTokenController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: ExternalUserToken
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.ExternalUserTokens.Include(e => e.Author).Include(e => e.User);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: ExternalUserToken/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ExternalUserTokens == null)
            {
                return NotFound();
            }

            var externalUserToken = await _context.ExternalUserTokens
                .Include(e => e.Author)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (externalUserToken == null)
            {
                return NotFound();
            }

            return View(externalUserToken);
        }

        // GET: ExternalUserToken/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ExternalUserToken/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccessToken,RefreshToken,ExpiresIn,IssuedAt,Scope,TokenType,UserId,AuthorId,Id")] ExternalUserToken externalUserToken)
        {
            if (ModelState.IsValid)
            {
                externalUserToken.Id = Guid.NewGuid();
                _context.Add(externalUserToken);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", externalUserToken.AuthorId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", externalUserToken.UserId);
            return View(externalUserToken);
        }

        // GET: ExternalUserToken/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ExternalUserTokens == null)
            {
                return NotFound();
            }

            var externalUserToken = await _context.ExternalUserTokens.FindAsync(id);
            if (externalUserToken == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", externalUserToken.AuthorId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", externalUserToken.UserId);
            return View(externalUserToken);
        }

        // POST: ExternalUserToken/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AccessToken,RefreshToken,ExpiresIn,IssuedAt,Scope,TokenType,UserId,AuthorId,Id")] ExternalUserToken externalUserToken)
        {
            if (id != externalUserToken.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(externalUserToken);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExternalUserTokenExists(externalUserToken.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", externalUserToken.AuthorId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", externalUserToken.UserId);
            return View(externalUserToken);
        }

        // GET: ExternalUserToken/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ExternalUserTokens == null)
            {
                return NotFound();
            }

            var externalUserToken = await _context.ExternalUserTokens
                .Include(e => e.Author)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (externalUserToken == null)
            {
                return NotFound();
            }

            return View(externalUserToken);
        }

        // POST: ExternalUserToken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ExternalUserTokens == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.ExternalUserTokens'  is null.");
            }
            var externalUserToken = await _context.ExternalUserTokens.FindAsync(id);
            if (externalUserToken != null)
            {
                _context.ExternalUserTokens.Remove(externalUserToken);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExternalUserTokenExists(Guid id)
        {
          return (_context.ExternalUserTokens?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
