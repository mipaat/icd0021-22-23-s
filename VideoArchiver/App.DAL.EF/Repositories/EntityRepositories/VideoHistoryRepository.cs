using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class VideoHistoryRepository : BaseAppEntityRepository<App.Domain.VideoHistory, VideoHistory>,
    IVideoHistoryRepository
{
    public VideoHistoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}