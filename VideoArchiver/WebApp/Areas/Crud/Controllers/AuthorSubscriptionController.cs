using App.Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorSubscriptionController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public AuthorSubscriptionController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AuthorSubscription
        public async Task<IActionResult> Index()
        {
            return View(await _uow.AuthorSubscriptions.GetAllAsync());
        }

        // GET: AuthorSubscription/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorSubscription = await _uow.AuthorSubscriptions.GetByIdAsync(id.Value);
            if (authorSubscription == null)
            {
                return NotFound();
            }

            return View(authorSubscription);
        }

        private async Task SetupViewData(AuthorSubscription? authorSubscription = null)
        {
            var authors = await _uow.Authors.GetAllAsync();
            ViewData["SubscriberId"] = new SelectList(authors, "Id", "IdOnPlatform", authorSubscription?.SubscriberId);
            ViewData["SubscriptionTargetId"] = new SelectList(authors, "Id", "IdOnPlatform", authorSubscription?.SubscriptionTargetId);
        }

        // GET: AuthorSubscription/Create
        public async Task<IActionResult> Create()
        {
            await SetupViewData();
            return View();
        }

        // POST: AuthorSubscription/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Platform,SubscriberId,SubscriptionTargetId,LastFetched,Priority,Id")] AuthorSubscription authorSubscription)
        {
            if (ModelState.IsValid)
            {
                authorSubscription.Id = Guid.NewGuid();
                _uow.AuthorSubscriptions.Add(authorSubscription);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(authorSubscription);
            return View(authorSubscription);
        }

        // GET: AuthorSubscription/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorSubscription = await _uow.AuthorSubscriptions.GetByIdAsync(id.Value);
            if (authorSubscription == null)
            {
                return NotFound();
            }

            await SetupViewData(authorSubscription);
            return View(authorSubscription);
        }

        // POST: AuthorSubscription/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Platform,SubscriberId,SubscriptionTargetId,LastFetched,Priority,Id")] AuthorSubscription authorSubscription)
        {
            if (id != authorSubscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.AuthorSubscriptions.Update(authorSubscription);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.AuthorSubscriptions.ExistsAsync(authorSubscription.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData(authorSubscription);
            return View(authorSubscription);
        }

        // GET: AuthorSubscription/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorSubscription = await _uow.AuthorSubscriptions.GetByIdAsync(id.Value);
            if (authorSubscription == null)
            {
                return NotFound();
            }

            return View(authorSubscription);
        }

        // POST: AuthorSubscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.AuthorSubscriptions.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
