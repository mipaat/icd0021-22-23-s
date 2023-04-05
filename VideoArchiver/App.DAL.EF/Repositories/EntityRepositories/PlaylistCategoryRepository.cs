using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class PlaylistCategoryRepository : BaseEntityRepository<PlaylistCategory, AbstractAppDbContext>,
    IPlaylistCategoryRepository
{
    public PlaylistCategoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<PlaylistCategory> DefaultIncludes(DbSet<PlaylistCategory> entities)
    {
        entities
            .Include(p => p.Category)
            .Include(p => p.Playlist);
        return entities;
    }
}