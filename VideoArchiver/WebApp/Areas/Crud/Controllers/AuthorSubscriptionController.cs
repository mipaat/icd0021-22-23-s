using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorSubscriptionController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public AuthorSubscriptionController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: AuthorSubscription
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.AuthorSubscriptions.Include(a => a.Subscriber).Include(a => a.SubscriptionTarget);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: AuthorSubscription/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.AuthorSubscriptions == null)
            {
                return NotFound();
            }

            var authorSubscription = await _context.AuthorSubscriptions
                .Include(a => a.Subscriber)
                .Include(a => a.SubscriptionTarget)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorSubscription == null)
            {
                return NotFound();
            }

            return View(authorSubscription);
        }

        // GET: AuthorSubscription/Create
        public IActionResult Create()
        {
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["SubscriptionTargetId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            return View();
        }

        // POST: AuthorSubscription/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Platform,SubscriberId,SubscriptionTargetId,LastFetched,Priority,Id")] AuthorSubscription authorSubscription)
        {
            if (ModelState.IsValid)
            {
                authorSubscription.Id = Guid.NewGuid();
                _context.Add(authorSubscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorSubscription.SubscriberId);
            ViewData["SubscriptionTargetId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorSubscription.SubscriptionTargetId);
            return View(authorSubscription);
        }

        // GET: AuthorSubscription/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.AuthorSubscriptions == null)
            {
                return NotFound();
            }

            var authorSubscription = await _context.AuthorSubscriptions.FindAsync(id);
            if (authorSubscription == null)
            {
                return NotFound();
            }
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorSubscription.SubscriberId);
            ViewData["SubscriptionTargetId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorSubscription.SubscriptionTargetId);
            return View(authorSubscription);
        }

        // POST: AuthorSubscription/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Platform,SubscriberId,SubscriptionTargetId,LastFetched,Priority,Id")] AuthorSubscription authorSubscription)
        {
            if (id != authorSubscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorSubscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorSubscriptionExists(authorSubscription.Id))
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
            ViewData["SubscriberId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorSubscription.SubscriberId);
            ViewData["SubscriptionTargetId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", authorSubscription.SubscriptionTargetId);
            return View(authorSubscription);
        }

        // GET: AuthorSubscription/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.AuthorSubscriptions == null)
            {
                return NotFound();
            }

            var authorSubscription = await _context.AuthorSubscriptions
                .Include(a => a.Subscriber)
                .Include(a => a.SubscriptionTarget)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorSubscription == null)
            {
                return NotFound();
            }

            return View(authorSubscription);
        }

        // POST: AuthorSubscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.AuthorSubscriptions == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.AuthorSubscriptions'  is null.");
            }
            var authorSubscription = await _context.AuthorSubscriptions.FindAsync(id);
            if (authorSubscription != null)
            {
                _context.AuthorSubscriptions.Remove(authorSubscription);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorSubscriptionExists(Guid id)
        {
          return (_context.AuthorSubscriptions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
