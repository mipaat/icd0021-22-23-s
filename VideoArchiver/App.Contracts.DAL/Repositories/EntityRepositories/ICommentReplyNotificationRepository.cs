using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface ICommentReplyNotificationRepository : IBaseEntityRepository<CommentReplyNotification, App.DAL.DTO.Entities.CommentReplyNotification>
{
}