using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoAuthorRepository : BaseEntityRepository<VideoAuthor, AbstractAppDbContext>, IVideoAuthorRepository
{
    public VideoAuthorRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<VideoAuthor> DefaultIncludes(DbSet<VideoAuthor> entities)
    {
        entities
            .Include(v => v.Author)
            .Include(v => v.Video);
        return entities;
    }
}