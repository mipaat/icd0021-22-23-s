using App.Contracts.DAL;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class VideoGameController : BaseEntityCrudController<IAppUnitOfWork, VideoGame>
    {
        public VideoGameController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<VideoGame, Guid> Entities => Uow.VideoGames;

        protected override async Task SetupViewData(VideoGame? entity = null)
        {
            ViewData["GameId"] = new SelectList(await Uow.Games.GetAllAsync(), "Id", "IgdbId", entity?.GameId);
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "VideoId,GameId,IgdbId,Platform,IdOnPlatform,Name,BoxArtUrl,FromTimecode,ToTimecode,ValidSince,ValidUntil,Id")]
            VideoGame entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "VideoId,GameId,IgdbId,Platform,IdOnPlatform,Name,BoxArtUrl,FromTimecode,ToTimecode,ValidSince,ValidUntil,Id")]
            VideoGame entity)
        {
            return await EditInternal(id, entity);
        }
    }
}