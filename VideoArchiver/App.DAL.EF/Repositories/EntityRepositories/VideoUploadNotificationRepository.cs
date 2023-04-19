using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class VideoUploadNotificationRepository : BaseEntityRepository<VideoUploadNotification, AbstractAppDbContext>,
    IVideoUploadNotificationRepository
{
    public VideoUploadNotificationRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<VideoUploadNotification> DefaultIncludes(DbSet<VideoUploadNotification> entities)
    {
        entities
            .Include(v => v.Receiver)
            .Include(v => v.Video);
        return entities;
    }
}