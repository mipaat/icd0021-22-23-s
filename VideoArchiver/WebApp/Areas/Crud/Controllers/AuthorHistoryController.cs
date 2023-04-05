using App.Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorHistoryController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public AuthorHistoryController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AuthorHistory
        public async Task<IActionResult> Index()
        {
            return View(await _uow.AuthorHistories.GetAllAsync());
        }

        // GET: AuthorHistory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorHistory = await _uow.AuthorHistories.GetByIdAsync(id.Value);
            if (authorHistory == null)
            {
                return NotFound();
            }

            return View(authorHistory);
        }

        // GET: AuthorHistory/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform");
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
                _uow.AuthorHistories.Add(authorHistory);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", authorHistory.AuthorId);
            return View(authorHistory);
        }

        // GET: AuthorHistory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorHistory = await _uow.AuthorHistories.GetByIdAsync(id.Value);
            if (authorHistory == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", authorHistory.AuthorId);
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
                    _uow.AuthorHistories.Update(authorHistory);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.AuthorHistories.ExistsAsync(authorHistory.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", authorHistory.AuthorId);
            return View(authorHistory);
        }

        // GET: AuthorHistory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorHistory = await _uow.AuthorHistories.GetByIdAsync(id.Value);
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
            await _uow.AuthorHistories.RemoveAsync(id);

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
