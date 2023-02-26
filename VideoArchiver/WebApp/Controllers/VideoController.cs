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
    public class VideoController : Controller
    {
        private readonly AbstractAppDbContext _context;

        public VideoController(AbstractAppDbContext context)
        {
            _context = context;
        }

        // GET: Video
        public async Task<IActionResult> Index()
        {
              return _context.Videos != null ? 
                          View(await _context.Videos.ToListAsync()) :
                          Problem("Entity set 'AbstractAppDbContext.Videos'  is null.");
        }

        // GET: Video/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Video/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Video/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Platform,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")] Video video)
        {
            if (ModelState.IsValid)
            {
                video.Id = Guid.NewGuid();
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        // GET: Video/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        // POST: Video/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Platform,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")] Video video)
        {
            if (id != video.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.Id))
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
            return View(video);
        }

        // GET: Video/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Videos == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Video/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Videos == null)
            {
                return Problem("Entity set 'AbstractAppDbContext.Videos'  is null.");
            }
            var video = await _context.Videos.FindAsync(id);
            if (video != null)
            {
                _context.Videos.Remove(video);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(Guid id)
        {
          return (_context.Videos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
