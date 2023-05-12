using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IStatusChangeNotificationRepository : IBaseEntityRepository<StatusChangeNotification, App.DAL.DTO.Entities.StatusChangeNotification>
{
}