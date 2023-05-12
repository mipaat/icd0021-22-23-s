using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IStatusChangeEventRepository : IBaseEntityRepository<StatusChangeEvent, App.DAL.DTO.Entities.StatusChangeEvent>
{
}