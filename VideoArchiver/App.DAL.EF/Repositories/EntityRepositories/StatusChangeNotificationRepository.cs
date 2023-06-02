using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class StatusChangeNotificationRepository :
    BaseAppEntityRepository<App.Domain.StatusChangeNotification, StatusChangeNotification>,
    IStatusChangeNotificationRepository
{
    public StatusChangeNotificationRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }
}