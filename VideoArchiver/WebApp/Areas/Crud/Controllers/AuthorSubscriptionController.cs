using App.Contracts.DAL;
using App.Domain;
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
    public class AuthorSubscriptionController : BaseEntityCrudController<IAppUnitOfWork, AuthorSubscription>
    {
        public AuthorSubscriptionController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<AuthorSubscription, Guid> Entities => Uow.AuthorSubscriptions;

        protected override async Task SetupViewData(AuthorSubscription? entity = null)
        {
            var authors = await Uow.Authors.GetAllAsync();
            ViewData["SubscriberId"] = new SelectList(authors, "Id", "IdOnPlatform", entity?.SubscriberId);
            ViewData["SubscriptionTargetId"] =
                new SelectList(authors, "Id", "IdOnPlatform", entity?.SubscriptionTargetId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Platform,SubscriberId,SubscriptionTargetId,LastFetched,Priority,Id")]
            AuthorSubscription entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Platform,SubscriberId,SubscriptionTargetId,LastFetched,Priority,Id")]
            AuthorSubscription entity)
        {
            return await EditInternal(id, entity);
        }
    }
}