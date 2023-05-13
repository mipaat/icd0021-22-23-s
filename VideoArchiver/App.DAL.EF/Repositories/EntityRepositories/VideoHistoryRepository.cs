using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoHistoryRepository : BaseAppEntityRepository<App.Domain.VideoHistory, VideoHistory>,
    IVideoHistoryRepository
{
    public VideoHistoryRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}