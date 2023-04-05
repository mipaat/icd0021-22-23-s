using App.Contracts.DAL.Repositories.EntityRepositories;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class CommentReplyNotificationRepository : BaseEntityRepository<CommentReplyNotification, AbstractAppDbContext>,
    ICommentReplyNotificationRepository
{
    public CommentReplyNotificationRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<CommentReplyNotification> DefaultIncludes(DbSet<CommentReplyNotification> entities)
    {
        entities
            .Include(c => c.Comment)
            .Include(c => c.Receiver)
            .Include(c => c.Reply);
        return entities;
    }
}