using App.Domain;
using Contracts.DAL;

namespace App.Contracts.DAL.Repositories.EntityRepositories;

public interface ICommentRepository : IBaseEntityRepository<Comment, App.DAL.DTO.Entities.Comment>
{
}