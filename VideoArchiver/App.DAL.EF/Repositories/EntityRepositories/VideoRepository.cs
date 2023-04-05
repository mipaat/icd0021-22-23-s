using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class VideoRepository : BaseEntityRepository<Video, AbstractAppDbContext>, IVideoRepository
{
    public VideoRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}