using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoHistoryRepository : IBaseEntityRepository<VideoHistory, App.DAL.DTO.Entities.VideoHistory>
{
}