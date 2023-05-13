using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace DAL.Repositories.EntityRepositories;

public class CommentReplyNotificationRepository : BaseAppEntityRepository<App.Domain.CommentReplyNotification, CommentReplyNotification>,
    ICommentReplyNotificationRepository
{
    public CommentReplyNotificationRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}