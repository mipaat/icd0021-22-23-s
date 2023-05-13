using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class VideoCategoryRepository : BaseAppEntityRepository<App.Domain.VideoCategory, VideoCategory>,
    IVideoCategoryRepository
{
    public VideoCategoryRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}