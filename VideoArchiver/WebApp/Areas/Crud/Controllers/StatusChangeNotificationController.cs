using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class StatusChangeNotificationController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public StatusChangeNotificationController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: StatusChangeNotification
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.StatusChangeNotifications.Include(s => s.Receiver).Include(s => s.StatusChangeEvent);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: StatusChangeNotification/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.StatusChangeNotifications == null)
            {
                return NotFound();
            }

            var statusChangeNotification = await _context.StatusChangeNotifications
                .Include(s => s.Receiver)
                .Include(s => s.StatusChangeEvent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusChangeNotification == null)
            {
                return NotFound();
            }

            return View(statusChangeNotification);
        }

        // GET: StatusChangeNotification/Create
        public IActionResult Create()
        {
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["StatusChangeEventId"] = new SelectList(_context.StatusChangeEvents, "Id", "Id");
            return View();
        }

        // POST: StatusChangeNotification/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiverId,StatusChangeEventId,SentAt,DeliveredAt,Id")] StatusChangeNotification statusChangeNotification)
        {
            if (ModelState.IsValid)
            {
                statusChangeNotification.Id = Guid.NewGuid();
                _context.Add(statusChangeNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", statusChangeNotification.ReceiverId);
            ViewData["StatusChangeEventId"] = new SelectList(_context.StatusChangeEvents, "Id", "Id", statusChangeNotification.StatusChangeEventId);
            return View(statusChangeNotification);
        }

        // GET: StatusChangeNotification/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.StatusChangeNotifications == null)
            {
                return NotFound();
            }

            var statusChangeNotification = await _context.StatusChangeNotifications.FindAsync(id);
            if (statusChangeNotification == null)
            {
                return NotFound();
            }
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", statusChangeNotification.ReceiverId);
            ViewData["StatusChangeEventId"] = new SelectList(_context.StatusChangeEvents, "Id", "Id", statusChangeNotification.StatusChangeEventId);
            return View(statusChangeNotification);
        }

        // POST: StatusChangeNotification/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReceiverId,StatusChangeEventId,SentAt,DeliveredAt,Id")] StatusChangeNotification statusChangeNotification)
        {
            if (id != statusChangeNotification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusChangeNotification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusChangeNotificationExists(statusChangeNotification.Id))
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
            ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id", statusChangeNotification.ReceiverId);
            ViewData["StatusChangeEventId"] = new SelectList(_context.StatusChangeEvents, "Id", "Id", statusChangeNotification.StatusChangeEventId);
            return View(statusChangeNotification);
        }

        // GET: StatusChangeNotification/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.StatusChangeNotifications == null)
            {
                return NotFound();
            }

            var statusChangeNotification = await _context.StatusChangeNotifications
                .Include(s => s.Receiver)
                .Include(s => s.StatusChangeEvent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusChangeNotification == null)
            {
                return NotFound();
            }

            return View(statusChangeNotification);
        }

        // POST: StatusChangeNotification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.StatusChangeNotifications == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.StatusChangeNotifications'  is null.");
            }
            var statusChangeNotification = await _context.StatusChangeNotifications.FindAsync(id);
            if (statusChangeNotification != null)
            {
                _context.StatusChangeNotifications.Remove(statusChangeNotification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusChangeNotificationExists(Guid id)
        {
          return (_context.StatusChangeNotifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
