using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoHistoryRepository : BaseEntityRepository<VideoHistory, AbstractAppDbContext>, IVideoHistoryRepository
{
    public VideoHistoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<VideoHistory> DefaultIncludes(DbSet<VideoHistory> entities)
    {
        entities.Include(v => v.Video);
        return entities;
    }
}