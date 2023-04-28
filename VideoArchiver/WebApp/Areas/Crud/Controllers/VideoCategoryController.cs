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
    public class VideoCategoryController : BaseEntityCrudController<IAppUnitOfWork, VideoCategory>
    {
        public VideoCategoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<VideoCategory, Guid> Entities => Uow.VideoCategories;

        protected override async Task SetupViewData(VideoCategory? entity = null)
        {
            ViewData["CategoryId"] = new SelectList(await Uow.Categories.GetAllAsync(), "Id", "Id", entity?.CategoryId);
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,CategoryId,AutoAssign,Id")] VideoCategory videoCategory)
        {
            return await CreateInternal(videoCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,CategoryId,AutoAssign,Id")] VideoCategory entity)
        {
            return await EditInternal(id, entity);
        }
    }
}