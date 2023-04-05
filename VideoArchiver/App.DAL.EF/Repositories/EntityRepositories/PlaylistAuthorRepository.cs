using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistAuthorRepository : BaseEntityRepository<PlaylistAuthor, AbstractAppDbContext>,
    IPlaylistAuthorRepository
{
    public PlaylistAuthorRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<PlaylistAuthor> DefaultIncludes(DbSet<PlaylistAuthor> entities)
    {
        entities
            .Include(p => p.Author)
            .Include(p => p.Playlist);
        return entities;
    }
}