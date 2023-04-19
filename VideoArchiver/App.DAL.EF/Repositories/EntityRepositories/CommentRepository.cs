using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class CommentRepository : BaseEntityRepository<Comment, AbstractAppDbContext>, ICommentRepository
{
    public CommentRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<Comment> DefaultIncludes(DbSet<Comment> entities)
    {
        entities
            .Include(c => c.Author)
            .Include(c => c.ConversationRoot)
            .Include(c => c.ReplyTarget)
            .Include(c => c.Video);
        return entities;
    }
}