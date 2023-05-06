using App.BLL.Exceptions;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.Domain;
using App.Domain.Enums;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class CommentService : BaseYouTubeService<CommentService>
{
    private readonly EntityUpdateHandler _entityUpdateHandler;
    
    public CommentService(YouTubeUow youTubeUow, ILogger<CommentService> logger, EntityUpdateHandler entityUpdateHandler) : base(youTubeUow, logger)
    {
        _entityUpdateHandler = entityUpdateHandler;
    }

    public async Task UpdateComments(Video video, CancellationToken ct)
    {
        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(video.IdOnPlatform, true, ct);
        await UpdateComments(video, videoData.Comments);
    }

    public async Task UpdateComments(string videoId, CancellationToken ct)
    {
        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(videoId, true, ct);

        Video? video = null;
        for (var i = 0; i < 3; i++)
        {
            video = await Uow.Videos.GetByIdOnPlatformWithCommentsAsync(videoId, Platform.YouTube);
            if (video == null) ct.WaitHandle.WaitOne(10000);
        }

        if (video == null)
        {
            throw new VideoNotFoundInArchiveException(videoId, Platform.YouTube);
        }

        await UpdateComments(video, videoData.Comments);
    }

    private async Task UpdateComments(Video video, CommentData[] comments)
    {
        // TODO: What to do if video has 20000 comments? Memory issues?
        video.Comments ??= new List<Comment>();
        var commentsWithoutParent = new List<(Comment Comment, string Parent)>();
        foreach (var comment in video.Comments)
        {
            if (comments.All(c => c.ID != comment.IdOnPlatform))
            {
                comment.DeletedAt = DateTime.UtcNow;
            }
        }
        foreach (var commentData in comments)
        {
            var domainComment = commentData.ToDomainComment();
            var existingDomainComment = video.Comments.SingleOrDefault(c => c.IdOnPlatform == commentData.ID);
            if (existingDomainComment != null)
            {
                _entityUpdateHandler.UpdateComment(existingDomainComment, domainComment);
                continue;
            }
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