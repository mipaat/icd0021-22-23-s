using App.Contracts.DAL;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class VideoUploadNotificationController : BaseEntityCrudController<IAppUnitOfWork, VideoUploadNotification>
    {
        public VideoUploadNotificationController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<VideoUploadNotification, Guid> Entities =>
            Uow.VideoUploadNotifications;

        protected override async Task SetupViewData(VideoUploadNotification? entity = null)
        {
            ViewData["ReceiverId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.ReceiverId);
            ViewData["VideoId"] =
                new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("VideoId,ReceiverId,Priority,SentAt,DeliveredAt,Id")]
            VideoUploadNotification entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("VideoId,ReceiverId,Priority,SentAt,DeliveredAt,Id")]
            VideoUploadNotification entity)
        {
            return await EditInternal(id, entity);
        }
    }
}