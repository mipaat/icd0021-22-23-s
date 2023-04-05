using App.Contracts.DAL;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorPubSubController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public AuthorPubSubController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AuthorPubSub
        public async Task<IActionResult> Index()
        {
            return View(await _uow.AuthorPubSubs.GetAllAsync());
        }

        // GET: AuthorPubSub/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorPubSub = await _uow.AuthorPubSubs.GetByIdAsync(id.Value);
            if (authorPubSub == null)
            {
                return NotFound();
            }

            return View(authorPubSub);
        }

        private async Task SetupViewData(AuthorPubSub? authorPubSub = null)
        {
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", authorPubSub?.AuthorId);
        }
        
        // GET: AuthorPubSub/Create
        public async Task<IActionResult> Create()
        {
            await SetupViewData();
            return View();
        }

        // POST: AuthorPubSub/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("LeasedAt,LeaseDuration,Secret,AuthorId,Id")] AuthorPubSub authorPubSub)
        {
            if (ModelState.IsValid)
            {
                authorPubSub.Id = Guid.NewGuid();
                _uow.AuthorPubSubs.Add(authorPubSub);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(authorPubSub);
            return View(authorPubSub);
        }

        // GET: AuthorPubSub/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorPubSub = await _uow.AuthorPubSubs.GetByIdAsync(id.Value);
            if (authorPubSub == null)
            {
                return NotFound();
            }

            await SetupViewData(authorPubSub);
            return View(authorPubSub);
        }

        // POST: AuthorPubSub/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("LeasedAt,LeaseDuration,Secret,AuthorId,Id")] AuthorPubSub authorPubSub)
        {
            if (id != authorPubSub.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.AuthorPubSubs.Update(authorPubSub);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.AuthorPubSubs.ExistsAsync(authorPubSub.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(authorPubSub);
            return View(authorPubSub);
        }

        // GET: AuthorPubSub/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorPubSub = await _uow.AuthorPubSubs.GetByIdAsync(id.Value);
            if (authorPubSub == null)
            {
                return NotFound();
            }

            return View(authorPubSub);
        }

        // POST: AuthorPubSub/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.AuthorPubSubs.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}