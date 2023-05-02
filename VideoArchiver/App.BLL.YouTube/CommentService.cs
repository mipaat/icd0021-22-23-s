using App.Domain;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube;

public class CommentService : BaseYouTubeService
{
    public CommentService(YouTubeUow youTubeUow) : base(youTubeUow)
    {
    }

    public async Task AddComments(Video video, IEnumerable<CommentData> comments)
    {
        video.Comments ??= new List<Comment>();
        var commentsWithoutParent = new List<(Comment Comment, string Parent)>();
        foreach (var commentData in comments)
        {
            var domainComment = commentData.ToDomainComment();
            await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(domainComment, commentData);
            domainComment.Video = video;
            if (commentData.Parent != "root")
            {
                var addedParentComment = video.Comments.SingleOrDefault(c => c.IdOnPlatform == commentData.Parent);
                if (addedParentComment == null)
                {
                    commentsWithoutParent.Add((domainComment, commentData.Parent));
                }
                else
                {
                    domainComment.ReplyTarget = addedParentComment;
                }
            }
            video.Comments.Add(domainComment);

            Uow.Comments.Add(domainComment);
        }

        foreach (var commentWithoutParent in commentsWithoutParent)
        {
            var parent = video.Comments.Single(c => c.IdOnPlatform == commentWithoutParent.Parent);
            commentWithoutParent.Comment.ReplyTarget = parent;
        }
    }
}