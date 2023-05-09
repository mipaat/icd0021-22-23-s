#pragma warning disable 1591
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
    public class PlaylistCategoryController : BaseEntityCrudController<IAppUnitOfWork, PlaylistCategory>
    {
        public PlaylistCategoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<PlaylistCategory, Guid> Entities => Uow.PlaylistCategories;

        protected override async Task SetupViewData(PlaylistCategory? entity = null)
        {
            ViewData["CategoryId"] =
                new SelectList(await Uow.Categories.GetAllAsync(), "Id", "Id", entity?.CategoryId);
            ViewData["PlaylistId"] =
                new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", entity?.PlaylistId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,CategoryId,AutoAssign,Id")] PlaylistCategory entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("PlaylistId,CategoryId,AutoAssign,Id")] PlaylistCategory entity)
        {
            return await EditInternal(id, entity);
        }
    }
}