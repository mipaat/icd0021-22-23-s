using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistHistoryRepository : BaseAppEntityRepository<App.Domain.PlaylistHistory, PlaylistHistory>, IPlaylistHistoryRepository
{
    public PlaylistHistoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}