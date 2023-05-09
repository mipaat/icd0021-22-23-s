#pragma warning disable 1591
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
    public class AuthorHistoryController : BaseEntityCrudController<IAppUnitOfWork, AuthorHistory>
    {
        public AuthorHistoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<AuthorHistory, Guid> Entities => Uow.AuthorHistories;

        protected override async Task SetupViewData(AuthorHistory? entity = null)
        {
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "AuthorId,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,LastValidAt,Id")]
            AuthorHistory entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "AuthorId,IdOnPlatform,UserName,DisplayName,Bio,ProfileImages,Banners,Thumbnails,CreatedAt,UpdatedAt,LastValidAt,Id")]
            AuthorHistory entity)
        {
            return await EditInternal(id, entity);
        }
    }
}