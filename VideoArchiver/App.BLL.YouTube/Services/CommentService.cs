using App.BLL.Exceptions;
using App.BLL.Services;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using YoutubeDLSharp.Metadata;

namespace App.BLL.YouTube.Services;

public class CommentService : BaseYouTubeService<CommentService>
{
    private readonly EntityUpdateService _entityUpdateService;

    public CommentService(ServiceUow serviceUow, ILogger<CommentService> logger, YouTubeUow youTubeUow,
        EntityUpdateService entityUpdateService, IMapper mapper) :
        base(serviceUow, logger, youTubeUow, mapper)
    {
        _entityUpdateService = entityUpdateService;
    }

    public async Task UpdateComments(string videoId, CancellationToken ct)
    {
        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(videoId, true, ct);
        var commentsFetched = DateTime.UtcNow;

        VideoWithComments? video = null;
        for (var i = 0; i < 3 && video == null; i++)
        {
            video = await Uow.Videos.GetByIdOnPlatformWithCommentsAsync(videoId, Platform.YouTube);
            if (video == null) ct.WaitHandle.WaitOne(10000);
        }

        if (video == null)
        {
            throw new VideoNotFoundInArchiveException(videoId, Platform.YouTube);
        }

        video.LastCommentsFetch = commentsFetched;

        await UpdateComments(video, videoData.Comments);
    }

    private async Task UpdateComments(VideoWithComments video, CommentData[] commentDatas)
    {
        // TODO: What to do if video has 20000 comments? Memory issues?
        var commentsWithoutParent = new List<(Comment Comment, string Parent)>();
        foreach (var comment in video.Comments)
        {
            if (commentDatas.All(c => c.ID != comment.IdOnPlatform))
            {
                comment.DeletedAt = DateTime.UtcNow;
            }
        }

        foreach (var commentData in commentDatas)
        {
            var comment = commentData.ToDalComment();
            var existingDomainComment = video.Comments.SingleOrDefault(c => c.IdOnPlatform == commentData.ID);
            if (existingDomainComment != null)
            {
                await _entityUpdateService.UpdateComment(existingDomainComment, comment);
                continue;
            }

            await YouTubeUow.AuthorService.AddAndSetAuthorIfNotSet(comment, commentData);
            comment.Video = video;
            comment.VideoId = video.Id;
            if (commentData.Parent != "root")
            {
                var addedParentComment = video.Comments.SingleOrDefault(c => c.IdOnPlatform == commentData.Parent);
                if (addedParentComment == null)
                {
                    commentsWithoutParent.Add((comment, commentData.Parent));
                }
                else
                {
                    comment.ReplyTarget = addedParentComment;
                }
            }

            video.Comments.Add(comment);

            Uow.Comments.Add(comment);
        }

        foreach (var commentWithoutParent in commentsWithoutParent)
        {
            var parent = video.Comments.Single(c => c.IdOnPlatform == commentWithoutParent.Parent);
            commentWithoutParent.Comment.ReplyTarget = parent;
        }
    }
}