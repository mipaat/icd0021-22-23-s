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
    public class PlaylistVideoController : BaseEntityCrudController<IAppUnitOfWork, PlaylistVideo>
    {
        public PlaylistVideoController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<PlaylistVideo, Guid> Entities => Uow.PlaylistVideos;

        protected override async Task SetupViewData(PlaylistVideo? entity = null)
        {
            var authors = await Uow.Authors.GetAllAsync();
            ViewData["AddedById"] = new SelectList(authors, "Id", "IdOnPlatform", entity?.AddedById);
            ViewData["PlaylistId"] =
                new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", entity?.PlaylistId);
            ViewData["RemovedById"] = new SelectList(authors, "Id", "IdOnPlatform", entity?.RemovedById);
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("PlaylistId,VideoId,Position,AddedAt,RemovedAt,AddedById,RemovedById,Id")] PlaylistVideo entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("PlaylistId,VideoId,Position,AddedAt,RemovedAt,AddedById,RemovedById,Id")] PlaylistVideo entity)
        {
            return await EditInternal(id, entity);
        }
    }
}