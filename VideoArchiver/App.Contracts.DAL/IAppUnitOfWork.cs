using App.Contracts.DAL.Repositories.EntityRepositories;
using Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IBaseUnitOfWork
{
    public IAuthorRepository Authors { get; }
    public IAuthorCategoryRepository AuthorCategories { get; }
    public ICategoryRepository Categories { get; }
    public IAuthorHistoryRepository AuthorHistories { get; }
    public IAuthorPubSubRepository AuthorPubSubs { get; }
    public IAuthorRatingRepository AuthorRatings { get; }
    public IAuthorSubscriptionRepository AuthorSubscriptions { get; }
    public ICommentRepository Comments { get; }
    public IVideoRepository Videos { get; }
    public ICommentReplyNotificationRepository CommentReplyNotifications { get; }
    public IExternalUserTokenRepository ExternalUserTokens { get; }
    public IGameRepository Games { get; }
}