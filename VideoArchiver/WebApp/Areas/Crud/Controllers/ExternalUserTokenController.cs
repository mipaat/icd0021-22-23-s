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
    public class ExternalUserTokenController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<User> _userManager;

        public ExternalUserTokenController(IAppUnitOfWork uow, UserManager<User> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        // GET: ExternalUserToken
        public async Task<IActionResult> Index()
        {
            return View(await _uow.ExternalUserTokens.GetAllAsync());
        }

        // GET: ExternalUserToken/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalUserToken = await _uow.ExternalUserTokens.GetByIdAsync(id.Value);
            if (externalUserToken == null)
            {
                return NotFound();
            }

            return View(externalUserToken);
        }

        private async Task SetupViewData()
        {
            ViewData["AuthorId"] = new SelectList(await _uow.Authors.GetAllAsync(), "Id", "IdOnPlatform");
            ViewData["UserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id");
        }
        
        // GET: ExternalUserToken/Create
        public async Task<IActionResult> Create()
        {
            await SetupViewData();
            return View();
        }

        // POST: ExternalUserToken/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccessToken,RefreshToken,ExpiresIn,IssuedAt,Scope,TokenType,UserId,AuthorId,Id")] ExternalUserToken externalUserToken)
        {
            if (ModelState.IsValid)
            {
                externalUserToken.Id = Guid.NewGuid();
                _uow.ExternalUserTokens.Add(externalUserToken);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData();
            return View(externalUserToken);
        }

        // GET: ExternalUserToken/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalUserToken = await _uow.ExternalUserTokens.GetByIdAsync(id.Value);
            if (externalUserToken == null)
            {
                return NotFound();
            }

            await SetupViewData();
            return View(externalUserToken);
        }

        // POST: ExternalUserToken/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AccessToken,RefreshToken,ExpiresIn,IssuedAt,Scope,TokenType,UserId,AuthorId,Id")] ExternalUserToken externalUserToken)
        {
            if (id != externalUserToken.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.ExternalUserTokens.Update(externalUserToken);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _uow.ExternalUserTokens.ExistsAsync(externalUserToken.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await SetupViewData();
            return View(externalUserToken);
        }

        // GET: ExternalUserToken/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalUserToken = await _uow.ExternalUserTokens.GetByIdAsync(id.Value);
            if (externalUserToken == null)
            {
                return NotFound();
            }

            return View(externalUserToken);
        }

        // POST: ExternalUserToken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.ExternalUserTokens.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
