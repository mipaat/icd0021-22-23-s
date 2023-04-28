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
    public class
        PlaylistVideoPositionHistoryController : BaseEntityCrudController<IAppUnitOfWork, PlaylistVideoPositionHistory>
    {
        public PlaylistVideoPositionHistoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<PlaylistVideoPositionHistory, Guid> Entities =>
            Uow.PlaylistVideoPositionHistories;

        protected override async Task SetupViewData(PlaylistVideoPositionHistory? entity = null)
        {
            ViewData["PlaylistVideoId"] = new SelectList(await Uow.PlaylistVideos.GetAllAsync(), "Id", "Id",
                entity?.PlaylistVideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("PlaylistVideoId,Position,ValidSince,ValidUntil,Id")]
            PlaylistVideoPositionHistory entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("PlaylistVideoId,Position,ValidSince,ValidUntil,Id")]
            PlaylistVideoPositionHistory entity)
        {
            return await EditInternal(id, entity);
        }
    }
}