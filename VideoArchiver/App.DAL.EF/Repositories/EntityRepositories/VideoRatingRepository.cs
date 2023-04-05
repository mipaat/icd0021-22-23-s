using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoRatingRepository : BaseEntityRepository<VideoRating, AbstractAppDbContext>, IVideoRatingRepository
{
    public VideoRatingRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<VideoRating> DefaultIncludes(DbSet<VideoRating> entities)
    {
        entities
            .Include(v => v.Author)
            .Include(v => v.Category)
            .Include(v => v.Video);
        return entities;
    }
}