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
    public class AuthorCategoryController : BaseEntityCrudController<IAppUnitOfWork, AuthorCategory>
    {
        public AuthorCategoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<AuthorCategory, Guid> Entities => Uow.AuthorCategories;

        protected override async Task SetupViewData(AuthorCategory? entity = null)
        {
            ViewData["AuthorId"] = new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["CategoryId"] = new SelectList(await Uow.Categories.GetAllAsync(), "Id", "Id", entity?.CategoryId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,CategoryId,AutoAssign,Id")] AuthorCategory entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AuthorId,CategoryId,AutoAssign,Id")] AuthorCategory entity)
        {
            return await EditInternal(id, entity);
        }
    }
}
