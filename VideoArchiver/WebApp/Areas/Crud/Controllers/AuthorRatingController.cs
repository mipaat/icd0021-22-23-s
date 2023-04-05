using App.Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorRatingController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public AuthorRatingController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AuthorRating
        public async Task<IActionResult> Index()
        {
            return View(await _uow.AuthorRatings.GetAllAsync());
        }

        // GET: AuthorRating/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorRating = await _uow.AuthorRatings.GetByIdAsync(id.Value);
            if (authorRating == null)
            {
                return NotFound();
            }

            return View(authorRating);
        }

        private async Task SetupViewData(AuthorRating? authorRating = null)
        {
            var authors = await _uow.Authors.GetAllAsync();
            ViewData["CategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", authorRating?.CategoryId);
            ViewData["RatedId"] = new SelectList(authors, "Id", "IdOnPlatform", authorRating?.RatedId);
            ViewData["RaterId"] = new SelectList(authors, "Id", "IdOnPlatform", authorRating?.RaterId);
        }

        // GET: AuthorRating/Create
        public async Task<IActionResult> Create()
        {
            await SetupViewData();
            return View();
        }

        // POST: AuthorRating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatedId,RaterId,Rating,Comment,CategoryId,Id")] AuthorRating authorRating)
        {
            if (ModelState.IsValid)
            {
                authorRating.Id = Guid.NewGuid();
                _uow.AuthorRatings.Add(authorRating);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(authorRating);
            return View(authorRating);
        }

        // GET: AuthorRating/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorRating = await _uow.AuthorRatings.GetByIdAsync(id.Value);
            if (authorRating == null)
            {
                return NotFound();
            }

            await SetupViewData(authorRating);
            return View(authorRating);
        }

        // POST: AuthorRating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RatedId,RaterId,Rating,Comment,CategoryId,Id")] AuthorRating authorRating)
        {
            if (id != authorRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.AuthorRatings.Update(authorRating);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.AuthorRatings.ExistsAsync(authorRating.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(authorRating);
            return View(authorRating);
        }

        // GET: AuthorRating/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorRating = await _uow.AuthorRatings.GetByIdAsync(id.Value);
            if (authorRating == null)
            {
                return NotFound();
            }

            return View(authorRating);
        }

        // POST: AuthorRating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.AuthorRatings.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
