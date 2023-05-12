using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistRatingRepository : BaseAppEntityRepository<App.Domain.PlaylistRating, PlaylistRating>,
    IPlaylistRatingRepository
{
    public PlaylistRatingRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}