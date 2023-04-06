using App.Contracts.DAL;
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
    public class CommentReplyNotificationController : BaseEntityCrudController<IAppUnitOfWork, CommentReplyNotification>
    {
        public CommentReplyNotificationController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<CommentReplyNotification, Guid> Entities =>
            Uow.CommentReplyNotifications;

        protected override async Task SetupViewData(CommentReplyNotification? entity = null)
        {
            var comments = await Uow.Comments.GetAllAsync();
            ViewData["CommentId"] = new SelectList(comments, "Id", "IdOnPlatform", entity?.CommentId);
            ViewData["ReceiverId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.ReceiverId);
            ViewData["ReplyId"] = new SelectList(comments, "Id", "IdOnPlatform", entity?.ReplyId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ReplyId,CommentId,ReceiverId,SentAt,DeliveredAt,Id")]
            CommentReplyNotification entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("ReplyId,CommentId,ReceiverId,SentAt,DeliveredAt,Id")]
            CommentReplyNotification entity)
        {
            return await EditInternal(id, entity);
        }
    }
}