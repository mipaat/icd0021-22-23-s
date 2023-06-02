using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class CommentRepository : BaseAppEntityRepository<App.Domain.Comment, Comment>, ICommentRepository
{
    public CommentRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext,
        mapper, uow)
    {
    }

    protected override TQueryable IncludeDefaults<TQueryable>(TQueryable queryable)
    {
        queryable.Include(e => e.ReplyTarget)
            .Include(e => e.ConversationRoot);
        return queryable;
    }

    protected override Domain.Comment AfterMap(Comment entity, Domain.Comment mapped)
    {
        if (entity.ReplyTarget != null)
        {
            var replyTarget = GetTrackedEntity(entity.ReplyTarget);
            if (replyTarget != null)
            {
                mapped.ReplyTarget = Map(entity.ReplyTarget, replyTarget);
            }
        }

        if (entity.ConversationRoot != null)
        {
            var conversationRoot = GetTrackedEntity(entity.ConversationRoot);
            if (conversationRoot != null)
            {
                mapped.ConversationRoot = Map(entity.ConversationRoot, conversationRoot);
            }
        }

        var author = Uow.Authors.GetTrackedEntity(entity.AuthorId);
        if (author != null)
        {
            mapped.Author = author;
        }

        var video = Uow.Videos.GetTrackedEntity(entity.VideoId);
        if (video != null)
        {
            mapped.Video = video;
        }

        return mapped;
    }

    public async Task<ICollection<CommentRoot>> GetCommentRootsForVideo(Guid videoId, int limit, int skipAmount = 0)
    {
        return await Entities
            .Where(c => c.VideoId == videoId && c.ConversationRootId == null)
            .OrderByDescending(c => c.CreatedAt)
            .ThenByDescending(c => c.Id)
            .Skip(skipAmount)
            .Take(limit)
            .Include(c => c.ConversationReplies)
            .ProjectTo<CommentRoot>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }
}