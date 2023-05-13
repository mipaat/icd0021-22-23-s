using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistSubscriptionRepository : BaseAppEntityRepository<App.Domain.PlaylistSubscription, PlaylistSubscription>,
    IPlaylistSubscriptionRepository
{
    public PlaylistSubscriptionRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}