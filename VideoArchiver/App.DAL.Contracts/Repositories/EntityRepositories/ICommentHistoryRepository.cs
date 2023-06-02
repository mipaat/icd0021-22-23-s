using App.Domain;
using Contracts.DAL;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface ICommentHistoryRepository : IBaseEntityRepository<CommentHistory, App.DAL.DTO.Entities.CommentHistory>
{
}