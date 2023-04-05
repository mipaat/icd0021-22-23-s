using App.Contracts.DAL;
using Base.WebHelpers;
using Contracts.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Crud.Controllers
{
    [Area("Crud")]
    public class PlaylistAuthorController : BaseEntityCrudController<IAppUnitOfWork, PlaylistAuthor>
    {
        public PlaylistAuthorController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<PlaylistAuthor, Guid> Entities => Uow.PlaylistAuthors;

        protected override async Task SetupViewData(PlaylistAuthor? entity = null)
        {
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["PlaylistId"] =
                new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", entity?.PlaylistId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,AuthorId,Role,Id")] PlaylistAuthor entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("PlaylistId,AuthorId,Role,Id")] PlaylistAuthor entity)
        {
            return await EditInternal(id, entity);
        }
    }
}