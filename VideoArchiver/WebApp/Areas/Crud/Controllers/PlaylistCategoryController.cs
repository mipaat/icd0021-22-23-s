using App.Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistCategoryController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public PlaylistCategoryController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: PlaylistCategory
        public async Task<IActionResult> Index()
        {
            return View(await _uow.PlaylistCategories.GetAllAsync());
        }

        // GET: PlaylistCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistCategory = await _uow.PlaylistCategories.GetByIdAsync(id.Value);
            if (playlistCategory == null)
            {
                return NotFound();
            }

            return View(playlistCategory);
        }

        private async Task SetupViewData(PlaylistCategory? playlistCategory = null)
        {
            ViewData["CategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", playlistCategory?.CategoryId);
            ViewData["PlaylistId"] = new SelectList(await _uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", playlistCategory?.PlaylistId);
        }

        // GET: PlaylistCategory/Create
        public async Task<IActionResult> Create()
        {
            await SetupViewData();
            return View();
        }

        // POST: PlaylistCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,CategoryId,AutoAssign,Id")] PlaylistCategory playlistCategory)
        {
            if (ModelState.IsValid)
            {
                playlistCategory.Id = Guid.NewGuid();
                _uow.PlaylistCategories.Add(playlistCategory);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(playlistCategory);
            return View(playlistCategory);
        }

        // GET: PlaylistCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistCategory = await _uow.PlaylistCategories.GetByIdAsync(id.Value);
            if (playlistCategory == null)
            {
                return NotFound();
            }

            await SetupViewData(playlistCategory);
            return View(playlistCategory);
        }

        // POST: PlaylistCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PlaylistId,CategoryId,AutoAssign,Id")] PlaylistCategory playlistCategory)
        {
            if (id != playlistCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.PlaylistCategories.Update(playlistCategory);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.PlaylistCategories.ExistsAsync(playlistCategory.Id))
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

            await SetupViewData(playlistCategory);
            return View(playlistCategory);
        }

        // GET: PlaylistCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistCategory = await _uow.PlaylistCategories.GetByIdAsync(id.Value);
            if (playlistCategory == null)
            {
                return NotFound();
            }

            return View(playlistCategory);
        }

        // POST: PlaylistCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.PlaylistCategories.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
