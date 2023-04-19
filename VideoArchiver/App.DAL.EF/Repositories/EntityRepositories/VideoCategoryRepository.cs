using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoCategoryRepository : BaseEntityRepository<VideoCategory, AbstractAppDbContext>,
    IVideoCategoryRepository
{
    public VideoCategoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<VideoCategory> DefaultIncludes(DbSet<VideoCategory> entities)
    {
        entities
            .Include(v => v.Category)
            .Include(v => v.Video);
        return entities;
    }
}