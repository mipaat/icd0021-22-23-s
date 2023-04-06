using App.Contracts.DAL;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    [Authorize(Roles = "Admin")]
    public class AuthorPubSubController : BaseEntityCrudController<IAppUnitOfWork, AuthorPubSub>
    {
        public AuthorPubSubController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<AuthorPubSub, Guid> Entities => Uow.AuthorPubSubs;

        protected override async Task SetupViewData(AuthorPubSub? entity = null)
        {
            ViewData["AuthorId"] = new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("LeasedAt,LeaseDuration,Secret,AuthorId,Id")] AuthorPubSub entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("LeasedAt,LeaseDuration,Secret,AuthorId,Id")] AuthorPubSub entity)
        {
            return await EditInternal(id, entity);
        }
    }
}