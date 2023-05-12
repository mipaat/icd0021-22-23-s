using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistVideoPositionHistoryRepository :
    BaseAppEntityRepository<App.Domain.PlaylistVideoPositionHistory, PlaylistVideoPositionHistory>, IPlaylistVideoPositionHistoryRepository
{
    public PlaylistVideoPositionHistoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}