using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class CommentReplyNotificationRepository : BaseAppEntityRepository<App.Domain.CommentReplyNotification, CommentReplyNotification>,
    ICommentReplyNotificationRepository
{
    public CommentReplyNotificationRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}