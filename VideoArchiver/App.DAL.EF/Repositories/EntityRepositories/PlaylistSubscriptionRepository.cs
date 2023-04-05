using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistSubscriptionRepository : BaseEntityRepository<PlaylistSubscription, AbstractAppDbContext>,
    IPlaylistSubscriptionRepository
{
    public PlaylistSubscriptionRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<PlaylistSubscription> DefaultIncludes(DbSet<PlaylistSubscription> entities)
    {
        entities
            .Include(p => p.Playlist)
            .Include(p => p.Subscriber);
        return entities;
    }
}