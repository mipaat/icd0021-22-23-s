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
    public class StatusChangeEventController : BaseEntityCrudController<IAppUnitOfWork, StatusChangeEvent>
    {
        public StatusChangeEventController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<StatusChangeEvent, Guid> Entities => Uow.StatusChangeEvents;

        protected override async Task SetupViewData(StatusChangeEvent? entity = null)
        {
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["PlaylistId"] =
                new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", entity?.PlaylistId);
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "PreviousAvailability,NewAvailability,PreviousPrivacyStatus,NewPrivacyStatus,OccurredAt,AuthorId,VideoId,PlaylistId,Id")]
            StatusChangeEvent entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "PreviousAvailability,NewAvailability,PreviousPrivacyStatus,NewPrivacyStatus,OccurredAt,AuthorId,VideoId,PlaylistId,Id")]
            StatusChangeEvent entity)
        {
            return await EditInternal(id, entity);
        }
    }
}