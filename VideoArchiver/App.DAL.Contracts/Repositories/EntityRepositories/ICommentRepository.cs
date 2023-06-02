using App.DAL.DTO.Entities;
using Contracts.DAL;
using Comment = App.Domain.Comment;

namespace App.DAL.Contracts.Repositories.EntityRepositories;

public interface ICommentRepository : IBaseEntityRepository<Comment, App.DAL.DTO.Entities.Comment>
{
    public Task<ICollection<CommentRoot>> GetCommentRootsForVideo(Guid videoId, int limit, int skipAmount = 0);
}