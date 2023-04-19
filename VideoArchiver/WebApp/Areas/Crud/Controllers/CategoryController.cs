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
    public class CategoryController : BaseEntityCrudController<IAppUnitOfWork, Category>
    {
        public CategoryController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<Category, Guid> Entities => Uow.Categories;

        protected override async Task SetupViewData(Category? entity = null)
        {
            ViewData["CreatorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.CreatorId);
            ViewData["ParentCategoryId"] =
                new SelectList(await Uow.Categories.GetAllAsync(), "Id", "Id", entity?.ParentCategoryId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Name,IsPublic,SupportsAuthors,SupportsVideos,SupportsPlaylists,IsAssignable,ParentCategoryId,Platform,CreatorId,Id")]
            Category entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "Name,IsPublic,SupportsAuthors,SupportsVideos,SupportsPlaylists,IsAssignable,ParentCategoryId,Platform,CreatorId,Id")]
            Category entity)
        {
            return await EditInternal(id, entity);
        }
    }
}