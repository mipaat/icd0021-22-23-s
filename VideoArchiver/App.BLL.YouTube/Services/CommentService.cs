using App.BLL.Exceptions;
using App.BLL.Services;
using App.BLL.YouTube.Base;
using App.BLL.YouTube.Extensions;
using App.DAL.DTO.Entities;
using App.Common.Enums;
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
        var maxComments = 800;
        var videoData = await YouTubeUow.VideoService.FetchVideoDataYtdl(videoId, true, maxComments, ct);
        var commentsFetched = DateTime.UtcNow;
        Logger.LogInformation("Fetched {commentLength} comments from YouTube for video {videoId}", videoData.Comments.Length, videoId);

        VideoWithComments? video = null;
        for (var i = 0; i < 3 && video == null; i++)
        {
            video = await Uow.Videos.GetByIdOnPlatformWithCommentsAsync(videoId, EPlatform.YouTube);
            if (video == null) ct.WaitHandle.WaitOne(10000);
        }

        if (video == null)
        {
            throw new VideoNotFoundInArchiveException(videoId, EPlatform.YouTube);
        }

        video.LastCommentsFetch = commentsFetched;

        await UpdateComments(video, videoData.Comments, maxComments);
    }

    private async Task UpdateComments(VideoWithComments video, CommentData[] commentDatas, int maxComments)
    {
        // TODO: What to do if video has 20000 comments? Memory issues?
        var commentsWithoutParent = new List<(Comment Comment, string Parent)>();
        if (commentDatas.Length < maxComments)
        {
            foreach (var comment in video.Comments)
            {
                if (commentDatas.All(c => c.ID != comment.IdOnPlatform))
                {
                    comment.DeletedAt = DateTime.UtcNow;
                }
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