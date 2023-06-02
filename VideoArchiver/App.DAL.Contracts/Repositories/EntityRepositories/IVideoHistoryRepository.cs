using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface IVideoHistoryRepository : IBaseEntityRepository<VideoHistory, App.DAL.DTO.Entities.VideoHistory>
{
}