using App.Contracts.DAL;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorCategoryController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public AuthorCategoryController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AuthorCategory
        public async Task<IActionResult> Index()
        {
            return View(await _uow.AuthorCategories.GetAllAsync());
        }

        // GET: AuthorCategory/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorCategory = await _uow.AuthorCategories.GetByIdAsync(id.Value);
            if (authorCategory == null)
            {
                return NotFound();
            }

            return View(authorCategory);
        }

        // GET: AuthorCategory/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform");
            ViewData["CategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id");
            return View();
        }

        // POST: AuthorCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,CategoryId,AutoAssign,Id")] AuthorCategory authorCategory)
        {
            if (ModelState.IsValid)
            {
                authorCategory.Id = Guid.NewGuid();
                _uow.AuthorCategories.Add(authorCategory);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", authorCategory.AuthorId);
            ViewData["CategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", authorCategory.CategoryId);
            return View(authorCategory);
        }

        // GET: AuthorCategory/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorCategory = await _uow.AuthorCategories.GetByIdAsync(id.Value);
            if (authorCategory == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", authorCategory.AuthorId);
            ViewData["CategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", authorCategory.CategoryId);
            return View(authorCategory);
        }

        // POST: AuthorCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AuthorId,CategoryId,AutoAssign,Id")] AuthorCategory authorCategory)
        {
            if (id != authorCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.AuthorCategories.Update(authorCategory);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.AuthorCategories.ExistsAsync(authorCategory.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", authorCategory.AuthorId);
            ViewData["CategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", authorCategory.CategoryId);
            return View(authorCategory);
        }

        // GET: AuthorCategory/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorCategory = await _uow.AuthorCategories.GetByIdAsync(id.Value);
            if (authorCategory == null)
            {
                return NotFound();
            }

            return View(authorCategory);
        }

        // POST: AuthorCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.AuthorCategories.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
