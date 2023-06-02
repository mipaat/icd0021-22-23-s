using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IStatusChangeEventRepository : IBaseEntityRepository<StatusChangeEvent, App.DAL.DTO.Entities.StatusChangeEvent>
{
}