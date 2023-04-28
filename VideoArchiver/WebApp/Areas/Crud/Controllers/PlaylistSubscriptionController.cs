using App.Contracts.DAL;
using App.Domain;
using App.DTO;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    [Authorize(Roles = RoleNames.Admin)]
    public class PlaylistSubscriptionController : BaseEntityCrudController<IAppUnitOfWork, PlaylistSubscription>
    {
        public PlaylistSubscriptionController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<PlaylistSubscription, Guid> Entities => Uow.PlaylistSubscriptions;

        protected override async Task SetupViewData(PlaylistSubscription? entity = null)
        {
            ViewData["PlaylistId"] =
                new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", entity?.PlaylistId);
            ViewData["SubscriberId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.SubscriberId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("PlaylistId,SubscriberId,Priority,Id")]
            PlaylistSubscription entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("PlaylistId,SubscriberId,Priority,Id")]
            PlaylistSubscription entity)
        {
            return await EditInternal(id, entity);
        }
    }
}