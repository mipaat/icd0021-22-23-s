using App.Contracts.DAL.Repositories.EntityRepositories;
using App.Domain;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.EntityRepositories;

public class CommentHistoryRepository : BaseEntityRepository<CommentHistory, AbstractAppDbContext>, ICommentHistoryRepository
{
    public CommentHistoryRepository(AbstractAppDbContext dbContext) : base(dbContext)
    {
    }

    protected override DbSet<CommentHistory> DefaultIncludes(DbSet<CommentHistory> entities)
    {
        entities.Include(ch => ch.Comment);
        return entities;
    }
}