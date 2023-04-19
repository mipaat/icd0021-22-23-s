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
    public class GameController : BaseEntityCrudController<IAppUnitOfWork, Game>
    {
        public GameController(IAppUnitOfWork uow) : base(uow)
        {
        }

        protected override IBaseEntityRepository<Game, Guid> Entities => Uow.Games;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IgdbId,Name,BoxArtUrl,Etag,LastFetched,LastSuccessfulFetch,Id")] Game entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("IgdbId,Name,BoxArtUrl,Etag,LastFetched,LastSuccessfulFetch,Id")] Game entity)
        {
            return await EditInternal(id, entity);
        }
    }
}