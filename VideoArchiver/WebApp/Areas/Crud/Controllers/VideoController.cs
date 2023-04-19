using App.Contracts.DAL;
using App.Domain;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    [Authorize(Roles = "Admin")]
    public class VideoController : BaseEntityCrudController<IAppUnitOfWork, Video>
    {
        public VideoController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<Video, Guid> Entities => Uow.Videos;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Platform,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")]
            Video entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "Platform,IdOnPlatform,Title,Description,DefaultLanguage,DefaultAudioLanguage,Duration,Width,Height,BitrateBps,ViewCount,LikeCount,DislikeCount,CommentCount,HasCaptions,Captions,Thumbnails,Tags,IsLivestreamRecording,StreamId,LivestreamStartedAt,LivestreamEndedAt,CreatedAt,PublishedAt,UpdatedAt,RecordedAt,LocalVideoFiles,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")]
            Video entity)
        {
            return await EditInternal(id, entity);
        }
    }
}