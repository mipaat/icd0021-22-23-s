using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoGameRepository : BaseAppEntityRepository<App.Domain.VideoGame, VideoGame>, IVideoGameRepository
{
    public VideoGameRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}