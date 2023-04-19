using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoGameRepository : BaseEntityRepository<VideoGame, AbstractAppDbContext>, IVideoGameRepository
{
    public VideoGameRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<VideoGame> DefaultIncludes(DbSet<VideoGame> entities)
    {
        entities
            .Include(v => v.Game)
            .Include(v => v.Video);
        return entities;
    }
}