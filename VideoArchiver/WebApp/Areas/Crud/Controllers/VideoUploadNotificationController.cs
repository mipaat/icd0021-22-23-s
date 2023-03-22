using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class VideoUploadNotificationController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public VideoUploadNotificationController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: VideoUploadNotification
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.VideoUploadNotifications.Include(v => v.Receiver).Include(v => v.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: VideoUploadNotification/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.VideoUploadNotifications == null)
            {
                return NotFound();
            }

            var videoUploadNotification = await _context.VideoUploadNotifications
                .Include(v => v.Receiver)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoUploadNotification == null)
            {
                return NotFound();
            }

            return View(videoUploadNotification);
        }

        // GET: VideoUploadNotification/Create
        public IActionResult Create()
        {
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform");
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: VideoUploadNotification/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,ReceiverId,Priority,SentAt,DeliveredAt,Id")] VideoUploadNotification videoUploadNotification)
        {
            if (ModelState.IsValid)
            {
                videoUploadNotification.Id = Guid.NewGuid();
                _context.Add(videoUploadNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoUploadNotification.ReceiverId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoUploadNotification.VideoId);
            return View(videoUploadNotification);
        }

        // GET: VideoUploadNotification/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.VideoUploadNotifications == null)
            {
                return NotFound();
            }

            var videoUploadNotification = await _context.VideoUploadNotifications.FindAsync(id);
            if (videoUploadNotification == null)
            {
                return NotFound();
            }
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoUploadNotification.ReceiverId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoUploadNotification.VideoId);
            return View(videoUploadNotification);
        }

        // POST: VideoUploadNotification/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,ReceiverId,Priority,SentAt,DeliveredAt,Id")] VideoUploadNotification videoUploadNotification)
        {
            if (id != videoUploadNotification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoUploadNotification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoUploadNotificationExists(videoUploadNotification.Id))
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
            ViewData["ReceiverId"] = new SelectList(_context.Authors, "Id", "IdOnPlatform", videoUploadNotification.ReceiverId);
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoUploadNotification.VideoId);
            return View(videoUploadNotification);
        }

        // GET: VideoUploadNotification/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.VideoUploadNotifications == null)
            {
                return NotFound();
            }

            var videoUploadNotification = await _context.VideoUploadNotifications
                .Include(v => v.Receiver)
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoUploadNotification == null)
            {
                return NotFound();
            }

            return View(videoUploadNotification);
        }

        // POST: VideoUploadNotification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.VideoUploadNotifications == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.VideoUploadNotifications'  is null.");
            }
            var videoUploadNotification = await _context.VideoUploadNotifications.FindAsync(id);
            if (videoUploadNotification != null)
            {
                _context.VideoUploadNotifications.Remove(videoUploadNotification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoUploadNotificationExists(Guid id)
        {
          return (_context.VideoUploadNotifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
