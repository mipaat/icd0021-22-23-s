using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoCategoryRepository : IBaseEntityRepository<VideoCategory, App.DAL.DTO.Entities.VideoCategory>
{
}