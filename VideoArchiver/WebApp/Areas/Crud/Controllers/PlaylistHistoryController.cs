using App.Contracts.DAL;
using App.Domain;
using App.DTO;
using Base.WebHelpers;
using Contracts.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    [Authorize(Roles = RoleNames.Admin)]
    public class PlaylistHistoryController : BaseEntityCrudController<IAppUnitOfWork, PlaylistHistory>
    {
        public PlaylistHistoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<PlaylistHistory, Guid> Entities => Uow.PlaylistHistories;

        protected override async Task SetupViewData(PlaylistHistory? entity = null)
        {
            ViewData["PlaylistId"] = new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "PlaylistId,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,LastValidAt,Id")]
            PlaylistHistory entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "PlaylistId,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,LastValidAt,Id")]
            PlaylistHistory entity)
        {
            return await EditInternal(id, entity);
        }
    }
}