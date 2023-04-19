using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class VideoRepository : BaseEntityRepository<Video, AbstractAppDbContext>, IVideoRepository
{
    public VideoRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}