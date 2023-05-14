using App.Contracts.DAL;
using App.Contracts.DAL.Repositories.EntityRepositories;
using App.DAL.DTO.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories;

public class CommentRepository : BaseAppEntityRepository<App.Domain.Comment, Comment>, ICommentRepository
{
    public CommentRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext,
        mapper, uow)
    {
    }

    protected override Func<TQueryable, TQueryable>? IncludeDefaultsFunc<TQueryable>()
    {
        return q =>
        {
            q
                .Include(e => e.ReplyTarget)
                .Include(e => e.ConversationRoot);
            return q;
        };
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

        if (entity.Author != null)
        {
            var author = Uow.Authors.GetTrackedEntity(entity.Author);
            if (author != null)
            {
                mapped.Author = Uow.Authors.Map(entity.Author, author);
            }
        }

        if (entity.Video != null)
        {
            var video = Uow.Videos.GetTrackedEntity(entity.Video);
            if (video != null)
            {
                mapped.Video = Uow.Videos.Map(entity.Video, video);
            }
        }

        return mapped;
    }
}