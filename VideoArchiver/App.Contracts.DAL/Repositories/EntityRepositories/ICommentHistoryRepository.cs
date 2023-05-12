using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface ICommentHistoryRepository : IBaseEntityRepository<CommentHistory, App.DAL.DTO.Entities.CommentHistory>
{
}