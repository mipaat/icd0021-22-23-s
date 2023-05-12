using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistVideoRepository : BaseAppEntityRepository<App.Domain.PlaylistVideo, PlaylistVideo>,
    IPlaylistVideoRepository
{
    public PlaylistVideoRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}