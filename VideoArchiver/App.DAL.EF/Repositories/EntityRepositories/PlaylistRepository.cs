using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistRepository : BaseEntityRepository<Playlist, AbstractAppDbContext>, IPlaylistRepository
{
    public PlaylistRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }
}