using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class StatusChangeNotificationRepository :
    BaseAppEntityRepository<App.Domain.StatusChangeNotification, StatusChangeNotification>,
    IStatusChangeNotificationRepository
{
    public StatusChangeNotificationRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}