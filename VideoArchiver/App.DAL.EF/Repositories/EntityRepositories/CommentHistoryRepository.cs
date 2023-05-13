using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class CommentHistoryRepository : BaseAppEntityRepository<App.Domain.CommentHistory, CommentHistory>, ICommentHistoryRepository
{
    public CommentHistoryRepository(AbstractAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}