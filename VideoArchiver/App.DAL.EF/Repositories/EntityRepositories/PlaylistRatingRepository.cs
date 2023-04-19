using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistRatingRepository : BaseEntityRepository<PlaylistRating, AbstractAppDbContext>,
    IPlaylistRatingRepository
{
    public PlaylistRatingRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<PlaylistRating> DefaultIncludes(DbSet<PlaylistRating> entities)
    {
        entities
            .Include(p => p.Author)
            .Include(p => p.Category)
            .Include(p => p.Playlist);
        return entities;
    }
}