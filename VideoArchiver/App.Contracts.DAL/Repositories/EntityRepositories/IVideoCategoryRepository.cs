using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoCategoryRepository : IBaseEntityRepository<App.Domain.VideoCategory, VideoCategory>
{
    public Task<bool> ExistsAsync(Guid categoryId, Guid videoId);
}