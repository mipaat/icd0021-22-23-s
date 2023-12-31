using App.BLL.Base;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Utils;

namespace App.BLL.Services;

public class CommentService : BaseService<CommentService>, ICommentService
{
    private readonly CommentMapper _commentMapper;
    private readonly VideoMapper _videoMapper;
    private readonly IEnumerable<IPlatformAuthorPresentationHandler> _authorPresentationHandlers;

    public CommentService(IServiceUow serviceUow, ILogger<CommentService> logger, IMapper mapper, IEnumerable<IPlatformAuthorPresentationHandler> authorPresentationHandlers) : base(serviceUow, logger)
    {
        _authorPresentationHandlers = authorPresentationHandlers;
        _commentMapper = new CommentMapper(mapper);
        _videoMapper = new VideoMapper(mapper);
    }

    public async Task<VideoWithAuthorAndComments> LoadVideoComments(VideoWithAuthor video, int limit, int page)
    {
        int? total = video.ArchivedRootCommentCount;
        var comments = await GetVideoComments(video.Id, limit, page, total);
        return _videoMapper.Map(video, comments);
    }

    public async Task<ICollection<Comment>> GetVideoComments(Guid videoId, int limit, int page, int? total)
    {
        PaginationUtils.ConformValues(ref total, ref limit, ref page);
        var skipAmount = PaginationUtils.PageToSkipAmount(limit, page);
        var dalComments = await Uow.Comments.GetCommentRootsForVideo(videoId, limit, skipAmount);
        var comments = dalComments.Select(c => _commentMapper.Map(c)).ToList();
        foreach (var comment in comments)
        {
            foreach (var authorHandler in _authorPresentationHandlers)
            {
                if (!authorHandler.CanHandle(comment.Author)) continue;
                comment.Author = authorHandler.Handle(comment.Author);
                break;
            }

            foreach (var reply in comment.Replies)
            {
                foreach (var authorHandler in _authorPresentationHandlers)
                {
                    if (!authorHandler.CanHandle(reply.Author)) continue;
                    reply.Author = authorHandler.Handle(reply.Author);
                    break;
                }
            }
        }

        return comments;
    }
}