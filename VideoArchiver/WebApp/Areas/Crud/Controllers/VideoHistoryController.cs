using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class VideoHistoryController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public VideoHistoryController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: VideoHistory
        public async Task<IActionResult> Index()
        {
            var abstractAppDbContext = _context.VideoHistories.Include(v => v.Video);
            return View(await abstractAppDbContext.ToListAsync());
        }

        // GET: VideoHistory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.VideoHistories == null)
            {
                return NotFound();
            }

            var videoHistory = await _context.VideoHistories
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoHistory == null)
            {
                return NotFound();
            }

            return View(videoHistory);
        }

        // GET: VideoHistory/Create
        public IActionResult Create()
        {
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform");
            return View();
        }

        // POST: VideoHistory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,LastValidAt,InternalPrivacyStatus,Id")] VideoHistory videoHistory)
        {
            if (ModelState.IsValid)
            {
                videoHistory.Id = Guid.NewGuid();
                _context.Add(videoHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoHistory.VideoId);
            return View(videoHistory);
        }

        // GET: VideoHistory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.VideoHistories == null)
            {
                return NotFound();
            }

            var videoHistory = await _context.VideoHistories.FindAsync(id);
            if (videoHistory == null)
            {
                return NotFound();
            }
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoHistory.VideoId);
            return View(videoHistory);
        }

        // POST: VideoHistory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,LastValidAt,InternalPrivacyStatus,Id")] VideoHistory videoHistory)
        {
            if (id != videoHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoHistoryExists(videoHistory.Id))
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
            ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "IdOnPlatform", videoHistory.VideoId);
            return View(videoHistory);
        }

        // GET: VideoHistory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.VideoHistories == null)
            {
                return NotFound();
            }

            var videoHistory = await _context.VideoHistories
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoHistory == null)
            {
                return NotFound();
            }

            return View(videoHistory);
        }

        // POST: VideoHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.VideoHistories == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.VideoHistories'  is null.");
            }
            var videoHistory = await _context.VideoHistories.FindAsync(id);
            if (videoHistory != null)
            {
                _context.VideoHistories.Remove(videoHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoHistoryExists(Guid id)
        {
          return (_context.VideoHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
