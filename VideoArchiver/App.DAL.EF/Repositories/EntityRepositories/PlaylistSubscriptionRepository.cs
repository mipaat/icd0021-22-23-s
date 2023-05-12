using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistSubscriptionRepository : BaseAppEntityRepository<App.Domain.PlaylistSubscription, PlaylistSubscription>,
    IPlaylistSubscriptionRepository
{
    public PlaylistSubscriptionRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}