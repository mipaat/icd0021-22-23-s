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
    public class StatusChangeNotificationController : BaseEntityCrudController<IAppUnitOfWork, StatusChangeNotification>
    {
        private readonly UserManager<User> _userManager;

        public StatusChangeNotificationController(IAppUnitOfWork uow, UserManager<User> userManager) : base(uow)
        {
            _userManager = userManager;
        }

        protected override IBaseEntityRepository<StatusChangeNotification, Guid> Entities =>
            Uow.StatusChangeNotifications;

        protected override async Task SetupViewData(StatusChangeNotification? entity = null)
        {
            ViewData["ReceiverId"] =
                new SelectList(await _userManager.Users.ToListAsync(), "Id", "Id", entity?.ReceiverId);
            ViewData["StatusChangeEventId"] = new SelectList(await Uow.StatusChangeEvents.GetAllAsync(), "Id", "Id",
                entity?.StatusChangeEventId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ReceiverId,StatusChangeEventId,SentAt,DeliveredAt,Id")]
            StatusChangeNotification entity)
        {
            return await CreateInternal(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("ReceiverId,StatusChangeEventId,SentAt,DeliveredAt,Id")]
            StatusChangeNotification entity)
        {
            return await EditInternal(id, entity);
        }
    }
}