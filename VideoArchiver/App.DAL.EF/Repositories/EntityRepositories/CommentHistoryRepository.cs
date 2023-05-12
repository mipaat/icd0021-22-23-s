using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using Contracts.DAL;

namespace DAL.Repositories.EntityRepositories;

public class CommentHistoryRepository : BaseAppEntityRepository<App.Domain.CommentHistory, CommentHistory>, ICommentHistoryRepository
{
    public CommentHistoryRepository(AbstractAppDbContext dbContext, ITrackingAutoMapperWrapper mapper) : base(dbContext, mapper)
    {
    }
}