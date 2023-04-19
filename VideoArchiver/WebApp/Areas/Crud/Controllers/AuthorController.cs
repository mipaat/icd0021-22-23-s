using App.Contracts.DAL;
using App.Domain;
using App.Domain.Identity;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    [Authorize(Roles = "Admin")]
    public class AuthorController : BaseEntityCrudController<IAppUnitOfWork, Author>
    {
        private readonly UserManager<User> _userManager;

        public AuthorController(IAppUnitOfWork uow, UserManager<User> userManager) : base(uow)
        {
            _userManager = userManager;
        }

        protected override IBaseEntityRepository<Author, Guid> Entities => Uow.Authors;

        protected override async Task SetupViewData(Author? entity = null)
        {
            ViewData["UserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", entity?.UserId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Platform,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,UserId,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")]
            Author entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "Platform,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,UserId,PrivacyStatus,IsAvailable,InternalPrivacyStatus,Etag,LastFetched,LastSuccessfulFetch,AddedToArchiveAt,Monitor,Download,Id")]
            Author entity)
        {
            return await EditInternal(id, entity);
        }
    }
}