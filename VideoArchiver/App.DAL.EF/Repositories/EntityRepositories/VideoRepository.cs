using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoRepository : BaseEntityRepository<Video, AbstractAppDbContext>, IVideoRepository
{
    public VideoRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Video?> GetByIdOnPlatformAsync(string idOnPlatform)
    {
        return await Entities.Where(v => v.IdOnPlatform == idOnPlatform).SingleOrDefaultAsync();
    }
}