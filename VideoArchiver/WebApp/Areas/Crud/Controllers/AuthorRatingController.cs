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
    public class AuthorRatingController : BaseEntityCrudController<IAppUnitOfWork, AuthorRating>
    {
        public AuthorRatingController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<AuthorRating, Guid> Entities => Uow.AuthorRatings;

        protected override async Task SetupViewData(AuthorRating? entity = null)
        {
            var authors = await Uow.Authors.GetAllAsync();
            ViewData["CategoryId"] =
                new SelectList(await Uow.Categories.GetAllAsync(), "Id", "Id", entity?.CategoryId);
            ViewData["RatedId"] = new SelectList(authors, "Id", "IdOnPlatform", entity?.RatedId);
            ViewData["RaterId"] = new SelectList(authors, "Id", "IdOnPlatform", entity?.RaterId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("RatedId,RaterId,Rating,Comment,CategoryId,Id")] AuthorRating entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("RatedId,RaterId,Rating,Comment,CategoryId,Id")] AuthorRating entity)
        {
            return await EditInternal(id, entity);
        }
    }
}