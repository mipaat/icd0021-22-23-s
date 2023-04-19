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
    public class PlaylistRatingController : BaseEntityCrudController<IAppUnitOfWork, PlaylistRating>
    {
        public PlaylistRatingController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<PlaylistRating, Guid> Entities => Uow.PlaylistRatings;

        protected override async Task SetupViewData(PlaylistRating? entity = null)
        {
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["CategoryId"] = new SelectList(await Uow.Categories.GetAllAsync(), "Id", "Id", entity?.CategoryId);
            ViewData["PlaylistId"] =
                new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", entity?.PlaylistId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("PlaylistId,AuthorId,Rating,Comment,CategoryId,Id")]
            PlaylistRating entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("PlaylistId,AuthorId,Rating,Comment,CategoryId,Id")]
            PlaylistRating entity)
        {
            return await EditInternal(id, entity);
        }
    }
}