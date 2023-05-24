using App.BLL.Base;
using App.BLL.DTO.Entities;
using App.BLL.DTO.Mappers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Utils;

namespace App.BLL.Services;

public class CommentService : BaseService<CommentService>
{
    private readonly CommentMapper _commentMapper;
    private readonly VideoMapper _videoMapper;
    
    public CommentService(ServiceUow serviceUow, ILogger<CommentService> logger, IMapper mapper) : base(serviceUow, logger, mapper)
    {
        _commentMapper = new CommentMapper(mapper);
        _videoMapper = new VideoMapper(mapper);
    }

    public async Task<VideoWithAuthorAndComments> LoadVideoComments(VideoWithAuthor video, int limit, int page)
    {
        var total = video.ArchivedRootCommentCount;
        PaginationUtils.ConformValues(ref total, ref limit, ref page);
        var skipAmount = PaginationUtils.PageToSkipAmount(limit, page);
        var dalComments = await Uow.Comments.GetCommentRootsForVideo(video.Id, limit, skipAmount);
        return _videoMapper.Map(video, dalComments.Select(c => _commentMapper.Map(c)).ToList());
    }
}