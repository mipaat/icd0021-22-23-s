using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class StatusChangeNotificationRepository :
    BaseAppEntityRepository<App.Domain.StatusChangeNotification, StatusChangeNotification>,
    IStatusChangeNotificationRepository
{
    public StatusChangeNotificationRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}