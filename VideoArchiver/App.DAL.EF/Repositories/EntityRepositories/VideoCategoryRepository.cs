using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class VideoCategoryRepository : BaseAppEntityRepository<App.Domain.VideoCategory, VideoCategory>,
    IVideoCategoryRepository
{
    public VideoCategoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}