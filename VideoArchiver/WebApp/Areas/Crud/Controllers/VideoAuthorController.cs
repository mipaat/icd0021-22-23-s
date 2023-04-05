using App.Contracts.DAL;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class VideoAuthorController : BaseEntityCrudController<IAppUnitOfWork, VideoAuthor>
    {
        public VideoAuthorController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<VideoAuthor, Guid> Entities => Uow.VideoAuthors;

        protected override async Task SetupViewData(VideoAuthor? entity = null)
        {
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,AuthorId,Role,Id")] VideoAuthor entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("VideoId,AuthorId,Role,Id")] VideoAuthor entity)
        {
            return await EditInternal(id, entity);
        }
    }
}