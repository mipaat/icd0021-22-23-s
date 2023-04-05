using App.Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistAuthorController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public PlaylistAuthorController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: PlaylistAuthor
        public async Task<IActionResult> Index()
        {
            return View(await _uow.PlaylistAuthors.GetAllAsync());
        }

        // GET: PlaylistAuthor/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistAuthor = await _uow.PlaylistAuthors.GetByIdAsync(id.Value);
            if (playlistAuthor == null)
            {
                return NotFound();
            }

            return View(playlistAuthor);
        }

        private async Task SetupViewData(PlaylistAuthor? playlistAuthor = null)
        {
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", playlistAuthor?.AuthorId);
            ViewData["PlaylistId"] = new SelectList(await _uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", playlistAuthor?.PlaylistId);
        }

        // GET: PlaylistAuthor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlaylistAuthor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,AuthorId,Role,Id")] PlaylistAuthor playlistAuthor)
        {
            if (ModelState.IsValid)
            {
                playlistAuthor.Id = Guid.NewGuid();
                _uow.PlaylistAuthors.Add(playlistAuthor);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(playlistAuthor);
            return View(playlistAuthor);
        }

        // GET: PlaylistAuthor/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistAuthor = await _uow.PlaylistAuthors.GetByIdAsync(id.Value);
            if (playlistAuthor == null)
            {
                return NotFound();
            }

            await SetupViewData(playlistAuthor);
            return View(playlistAuthor);
        }

        // POST: PlaylistAuthor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,AuthorId,Role,Id")] PlaylistAuthor playlistAuthor)
        {
            if (id != playlistAuthor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.PlaylistAuthors.Update(playlistAuthor);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.PlaylistAuthors.ExistsAsync(playlistAuthor.Id))
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

            await SetupViewData(playlistAuthor);
            return View(playlistAuthor);
        }

        // GET: PlaylistAuthor/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistAuthor = await _uow.PlaylistAuthors.GetByIdAsync(id.Value);
            if (playlistAuthor == null)
            {
                return NotFound();
            }

            return View(playlistAuthor);
        }

        // POST: PlaylistAuthor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.PlaylistAuthors.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
