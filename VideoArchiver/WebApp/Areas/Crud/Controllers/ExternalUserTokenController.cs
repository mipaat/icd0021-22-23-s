using App.Contracts.DAL;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    [Authorize(Roles = "Admin")]
    public class ExternalUserTokenController : BaseEntityCrudController<IAppUnitOfWork, ExternalUserToken>
    {
        private readonly UserManager<User> _userManager;

        public ExternalUserTokenController(IAppUnitOfWork uow, UserManager<User> userManager) : base(uow)
        {
            _userManager = userManager;
        }

        protected override IBaseEntityRepository<ExternalUserToken, Guid> Entities => Uow.ExternalUserTokens;

        protected override async Task SetupViewData(ExternalUserToken? entity = null)
        {
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["UserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", entity?.UserId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("AccessToken,RefreshToken,ExpiresIn,IssuedAt,Scope,TokenType,UserId,AuthorId,Id")]
            ExternalUserToken entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("AccessToken,RefreshToken,ExpiresIn,IssuedAt,Scope,TokenType,UserId,AuthorId,Id")]
            ExternalUserToken entity)
        {
            return await EditInternal(id, entity);
        }
    }
}