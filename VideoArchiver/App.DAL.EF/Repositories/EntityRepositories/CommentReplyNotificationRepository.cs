using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class CommentReplyNotificationRepository : BaseAppEntityRepository<App.Domain.CommentReplyNotification, CommentReplyNotification>,
    ICommentReplyNotificationRepository
{
    public CommentReplyNotificationRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}