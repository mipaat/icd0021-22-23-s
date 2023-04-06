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
    public class CommentController : BaseEntityCrudController<IAppUnitOfWork, Comment>
    {
        public CommentController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<Comment, Guid> Entities => Uow.Comments;

        protected override async Task SetupViewData(Comment? entity = null)
        {
            var comments = await Uow.Comments.GetAllAsync();
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["ConversationRootId"] = new SelectList(comments, "Id", "IdOnPlatform", entity?.ConversationRootId);
            ViewData["ReplyTargetId"] = new SelectList(comments, "Id", "IdOnPlatform", entity?.ReplyTargetId);
            ViewData["VideoId"] =
                new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Platform,IdOnPlatform,AuthorId,VideoId,ReplyTargetId,ConversationRootId,Content,LikeCount,DislikeCount,ReplyCount,CreatedAt,CreatedAtVideoTimecode,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,FetchSuccess,AddedToArchiveAt,Id")]
            Comment entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "Platform,IdOnPlatform,AuthorId,VideoId,ReplyTargetId,ConversationRootId,Content,LikeCount,DislikeCount,ReplyCount,CreatedAt,CreatedAtVideoTimecode,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,FetchSuccess,AddedToArchiveAt,Id")]
            Comment entity)
        {
            return await EditInternal(id, entity);
        }
    }
}