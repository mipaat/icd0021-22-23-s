using App.Contracts.DAL;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class CategoryController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public CategoryController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Categories.GetAllAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _uow.Categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CreatorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform");
            ViewData["ParentCategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id");
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsPublic,SupportsAuthors,SupportsVideos,SupportsPlaylists,IsAssignable,ParentCategoryId,Platform,CreatorId,Id")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                _uow.Categories.Add(category);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", category.CreatorId);
            ViewData["ParentCategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", category.ParentCategoryId);
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _uow.Categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["CreatorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", category.CreatorId);
            ViewData["ParentCategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", category.ParentCategoryId);
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,IsPublic,SupportsAuthors,SupportsVideos,SupportsPlaylists,IsAssignable,ParentCategoryId,Platform,CreatorId,Id")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Categories.Update(category);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Categories.ExistsAsync(category.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", category.CreatorId);
            ViewData["ParentCategoryId"] = new SelectList(await _uow.Categories.GetAllAsync(), "Id", "Id", category.ParentCategoryId);
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _uow.Categories.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Categories.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
