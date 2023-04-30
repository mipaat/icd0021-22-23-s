using App.Domain;
using App.Domain.Enums;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface IVideoRepository : IBaseEntityRepository<Video>
{
    public Task<Video?> GetByIdOnPlatformAsync(string idOnPlatform, Platform platform);
}