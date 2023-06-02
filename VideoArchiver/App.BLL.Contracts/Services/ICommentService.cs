using App.BLL.DTO.Entities;

namespace App.BLL.Contracts.Services;

public interface ICommentService : IBaseService
{
    public Task<VideoWithAuthorAndComments> LoadVideoComments(VideoWithAuthor video, int limit, int page);
    public Task<ICollection<Comment>> GetVideoComments(Guid videoId, int limit, int page, int? total);
}