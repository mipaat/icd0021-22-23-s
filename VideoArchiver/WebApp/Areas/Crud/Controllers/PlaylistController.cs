#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using App.DTO;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    [Authorize(Roles = RoleNames.Admin)]
    public class PlaylistController : BaseEntityCrudController<IAppUnitOfWork, Playlist>
    {
        public PlaylistController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<Playlist, Guid> Entities => Uow.Playlists;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Platform,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetchOfficial,LastSuccessfulFetchOfficial,LastFetchUnofficial,LastSuccessfulFetchUnofficial,AddedToArchiveAt,Monitor,Download,Id")]
            Playlist entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "Platform,IdOnPlatform,Title,Description,Thumbnails,Tags,CreatedAt,PublishedAt,UpdatedAt,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetchOfficial,LastSuccessfulFetchOfficial,LastFetchUnofficial,LastSuccessfulFetchUnofficial,AddedToArchiveAt,Monitor,Download,Id")]
            Playlist entity)
        {
            return await EditInternal(id, entity);
        }
    }
}