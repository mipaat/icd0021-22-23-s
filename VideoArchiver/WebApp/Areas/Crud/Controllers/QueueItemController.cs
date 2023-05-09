#pragma warning disable 1591
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Identity;
using App.DTO;
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
    [Authorize(Roles = RoleNames.Admin)]
    public class QueueItemController : BaseEntityCrudController<IAppUnitOfWork, QueueItem>
    {
        private readonly UserManager<User> _userManager;

        public QueueItemController(IAppUnitOfWork uow, UserManager<User> userManager) : base(uow)
        {
            _userManager = userManager;
        }

        protected override IBaseEntityRepository<QueueItem, Guid> Entities => Uow.QueueItems;

        protected override async Task SetupViewData(QueueItem? entity = null)
        {
            ViewData["AddedById"] =
                new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", entity?.AddedById);
            ViewData["ApprovedById"] =
                new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", entity?.ApprovedById);
            ViewData["AuthorId"] =
                new SelectList(await Uow.Authors.GetAllAsync(), "Id", "IdOnPlatform", entity?.AuthorId);
            ViewData["PlaylistId"] =
                new SelectList(await Uow.Playlists.GetAllAsync(), "Id", "IdOnPlatform", entity?.PlaylistId);
            ViewData["VideoId"] = new SelectList(await Uow.Videos.GetAllAsync(), "Id", "IdOnPlatform", entity?.VideoId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Url,Platform,IdOnPlatform,Monitor,Download,WebHookUrl,WebhookSecret,WebhookData,AddedById,AddedAt,ApprovedById,ApprovedAt,CompletedAt,AuthorId,VideoId,PlaylistId,Id")]
            QueueItem entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind(
                "Url,Platform,IdOnPlatform,Monitor,Download,WebHookUrl,WebhookSecret,WebhookData,AddedById,AddedAt,ApprovedById,ApprovedAt,CompletedAt,AuthorId,VideoId,PlaylistId,Id")]
            QueueItem entity)
        {
            return await EditInternal(id, entity);
        }
    }
}