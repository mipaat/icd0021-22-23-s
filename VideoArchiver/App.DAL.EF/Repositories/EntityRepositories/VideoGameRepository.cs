using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class VideoGameRepository : BaseAppEntityRepository<App.Domain.VideoGame, VideoGame>, IVideoGameRepository
{
    public VideoGameRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}