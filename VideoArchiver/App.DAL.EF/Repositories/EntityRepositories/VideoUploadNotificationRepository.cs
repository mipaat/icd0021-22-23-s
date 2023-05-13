using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class VideoUploadNotificationRepository :
    BaseAppEntityRepository<App.Domain.VideoUploadNotification, VideoUploadNotification>,
    IVideoUploadNotificationRepository
{
    public VideoUploadNotificationRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}