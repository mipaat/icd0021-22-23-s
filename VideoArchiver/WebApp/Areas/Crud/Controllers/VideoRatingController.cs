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
    public class VideoRatingController : BaseEntityCrudController<IAppUnitOfWork, VideoRating>
    {
        public VideoRatingController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<VideoRating, Guid> Entities => Uow.VideoRatings;

        protected override async Task SetupViewData(VideoRating? entity = null)
        {
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["CategoryId"] = new SelectList(await Uow.Categories.GetAllAsync(), "Id", "Id", entity?.CategoryId);
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("VideoId,AuthorId,Rating,Comment,CategoryId,Id")] VideoRating entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("VideoId,AuthorId,Rating,Comment,CategoryId,Id")] VideoRating entity)
        {
            return await EditInternal(id, entity);
        }
    }
}