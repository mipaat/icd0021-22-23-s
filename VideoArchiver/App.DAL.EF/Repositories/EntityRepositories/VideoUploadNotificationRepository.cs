using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class VideoUploadNotificationRepository :
    BaseAppEntityRepository<App.Domain.VideoUploadNotification, VideoUploadNotification>,
    IVideoUploadNotificationRepository
{
    public VideoUploadNotificationRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}