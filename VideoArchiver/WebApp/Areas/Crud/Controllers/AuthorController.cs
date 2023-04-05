using App.Contracts.DAL;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class AuthorController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<User> _userManager;

        public AuthorController(IAppUnitOfWork uow, UserManager<User> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        // GET: Author
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Authors.GetAllAsync());
        }

        // GET: Author/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _uow.Authors.GetByIdAsync(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Author/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Id");
            return View();
        }

        // POST: Author/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Platform,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,UserId,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")]
            Author author)
        {
            if (ModelState.IsValid)
            {
                author.Id = Guid.NewGuid();
                _uow.Authors.Add(author);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Id", author.UserId);
            return View(author);
        }

        // GET: Author/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _uow.Authors.GetByIdAsync(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Id", author.UserId);
            return View(author);
        }

        // POST: Author/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "Platform,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,UserId,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")]
            Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Authors.Update(author);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.Authors.ExistsAsync(author.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Id", author.UserId);
            return View(author);
        }

        // GET: Author/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _uow.Authors.GetByIdAsync(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Authors.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}