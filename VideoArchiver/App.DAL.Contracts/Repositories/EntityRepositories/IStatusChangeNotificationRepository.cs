using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IStatusChangeNotificationRepository : IBaseEntityRepository<StatusChangeNotification, App.DAL.DTO.Entities.StatusChangeNotification>
{
}