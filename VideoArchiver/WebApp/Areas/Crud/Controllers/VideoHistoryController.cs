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
    public class VideoHistoryController : BaseEntityCrudController<IAppUnitOfWork, VideoHistory>
    {
        public VideoHistoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<VideoHistory, Guid> Entities => Uow.VideoHistories;

        protected override async Task SetupViewData(VideoHistory? entity = null)
        {
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "VideoId,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,LastValidAt,InternalPrivacyStatus,Id")]
            VideoHistory entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "VideoId,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,LastValidAt,InternalPrivacyStatus,Id")]
            VideoHistory entity)
        {
            return await EditInternal(id, entity);
        }
    }
}