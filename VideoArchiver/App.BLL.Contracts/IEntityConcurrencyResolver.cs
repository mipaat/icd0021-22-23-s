using App.Domain;

namespace App.BLL.Contracts;

public interface IEntityConcurrencyResolver
{
    public Task<Video> ResolveVideoConcurrency(Video currentVideo, Video? dbVideo, Exception sourceException);

    public Task<Comment> ResolveCommentConcurrency(Comment currentComment, Comment? dbComment,
        Exception sourceException);
}