using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoRatingRepository : BaseAppEntityRepository<App.Domain.VideoRating, VideoRating>,
    IVideoRatingRepository
{
    public VideoRatingRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}